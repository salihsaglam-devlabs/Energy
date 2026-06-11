using System.Security.Claims;
using System.Globalization;
using Energy.Localization;
using Energy.Shared.Identity;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.Identity;
using SystemClients = Energy.Web.Clients.System;
using Energy.Web.Common;
using Energy.Web.Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Energy.Web.Services.Navigation;

public interface INavigationService
{
    Task<IReadOnlyList<NavigationItem>> GetMenuForUserAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Loads the menu tree from the API and intersects it with the menus assigned
/// to the current user's roles. Admins (resolved by role name) bypass the
/// intersection and see the full tree.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Menu URLs that are pinned to every authenticated user's navigation,
    /// regardless of their role/menu/permission configuration. <c>/dashboard</c>
    /// is the default landing surface and <c>/profile</c> is the per-user page
    /// every account must always be able to reach.
    /// </summary>
    private static readonly HashSet<string> AlwaysVisibleMenuUrls =
        new(StringComparer.OrdinalIgnoreCase) { "/dashboard", "/profile" };

    /// <summary>
    /// Hard-coded fallback navigation rendered when the API menu tree cannot
    /// be loaded for the current caller (e.g. they lack <c>Menu.GetMenuTree</c>
    /// permission). Guarantees that every authenticated user still gets the
    /// two per-user defaults in the drawer.
    /// </summary>
    private static readonly NavigationItem[] FallbackPinnedNavigation =
    [
        new()
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            ParentId = null,
            Name = "Menus.Dashboard",
            Url = "/dashboard",
            Icon = "home",
            Order = 1,
            RequiredPermissions = Array.Empty<string>()
        },
        new()
        {
            Id = new Guid("22222222-2222-2222-2222-222222222222"),
            ParentId = null,
            Name = "Menus.Profile",
            Url = "/profile",
            Icon = "user",
            Order = 2,
            RequiredPermissions = Array.Empty<string>()
        }
    ];

    private readonly SystemClients.IMenuApiClient _menuApiClient;
    private readonly IRoleApiClient _roleApiClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<NavigationService> _logger;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public NavigationService(
        SystemClients.IMenuApiClient menuApiClient,
        IRoleApiClient roleApiClient,
        IMemoryCache cache,
        ILogger<NavigationService> logger,
        IStringLocalizer<SharedResource> localizer)
    {
        _menuApiClient = menuApiClient;
        _roleApiClient = roleApiClient;
        _cache = cache;
        _logger = logger;
        _localizer = localizer;
    }

    private NavigationItem[] BuildLocalizedFallback()
    {
        return FallbackPinnedNavigation
            .Select(item => new NavigationItem
            {
                Id = item.Id,
                ParentId = item.ParentId,
                Name = _localizer.GetText(item.Name, item.Name),
                Url = item.Url,
                Icon = item.Icon,
                Order = item.Order,
                RequiredPermissions = item.RequiredPermissions
            })
            .OrderBy(item => item.Order)
            .ToArray();
    }

    public async Task<IReadOnlyList<NavigationItem>> GetMenuForUserAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.GetUserId();
        if (userId is null)
        {
            return Array.Empty<NavigationItem>();
        }

        var cacheKey = $"energy.nav.{userId}.{CultureInfo.CurrentUICulture.Name}";
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<NavigationItem>? cached) && cached is not null)
        {
            return cached;
        }

        // The menu tree API requires a per-user permission. When the caller
        // does not have it (e.g. a freshly seeded role missing the read perm),
        // surface no menu rather than letting the 403 bubble up and crash the
        // request pipeline. The page-access filter then sends the user to the
        // access-denied page with a clear hint instead of a 500.
        IReadOnlyList<MenuResponse>? menuTree = null;
        try
        {
            var treeEnvelope = await _menuApiClient.GetMenuTreeAsync(cancellationToken);
            if (treeEnvelope.IsSuccess && treeEnvelope.Data is not null)
            {
                menuTree = treeEnvelope.Data;
            }
        }
        catch (ApiForbiddenException)
        {
            _logger.LogWarning("User {UserId} cannot read the menu tree (forbidden); rendering an empty navigation.", userId);
        }
        catch (ApiUnauthorizedException)
        {
            // Auth filter / cookie middleware will handle this on the next hop.
            _logger.LogInformation("User {UserId} is no longer authorized for the menu tree.", userId);
        }

        if (menuTree is null)
        {
            // API refused or failed — still surface the per-user defaults so
            // the user has somewhere to go from the drawer.
            var fallback = BuildLocalizedFallback();
            _cache.Set(cacheKey, (IReadOnlyList<NavigationItem>)fallback, CacheDuration);
            return fallback;
        }

        var allItems = Flatten(menuTree);

        var isAdmin = user.HasRoleKey(SystemRoleKeys.Admin) || user.IsInRole("Admin");
        IReadOnlyList<NavigationItem> visible;

        if (isAdmin)
        {
            visible = allItems;
        }
        else
        {
            var allowedIds = await GetAllowedMenuIdsAsync(user, cancellationToken);
            var filtered = FilterByAllowedAndKeepAncestors(
                allItems,
                allowedIds,
                code => user.HasPermission(code));

            // Always pin the per-user defaults (Dashboard + Profile) on top of
            // the role-filtered list. Prefer the real seeded entries when they
            // exist in the tree so the menu uses the canonical localized name
            // and id; otherwise fall back to the synthetic ones above.
            var pinnedFromTree = allItems
                .Where(item => item.Url is not null && AlwaysVisibleMenuUrls.Contains(item.Url))
                .ToList();

            var pinnedUrls = pinnedFromTree
                .Select(item => item.Url!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var pinnedMissing = BuildLocalizedFallback()
                .Where(item => item.Url is not null && !pinnedUrls.Contains(item.Url));

            visible = filtered
                .Concat(pinnedFromTree)
                .Concat(pinnedMissing)
                .GroupBy(item => item.Id)
                .Select(group => group.First())
                .ToArray();
        }

        var ordered = visible
            .OrderBy(i => i.ParentId.HasValue)
            .ThenBy(i => i.Order)
            .ThenBy(i => i.Name)
            .ToArray();

        _cache.Set(cacheKey, (IReadOnlyList<NavigationItem>)ordered, CacheDuration);
        return ordered;
    }

    private async Task<HashSet<Guid>> GetAllowedMenuIdsAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var roleIds = user.GetRoleIds();
        var allowed = new HashSet<Guid>();

        foreach (var roleId in roleIds)
        {
            try
            {
                var envelope = await _roleApiClient.GetRoleMenusAsync(
                    roleId,
                    new PaginatedRequest { PageNumber = 1, PageSize = 100 },
                    cancellationToken);

                if (!envelope.IsSuccess || envelope.Data is null)
                {
                    continue;
                }

                foreach (var menu in envelope.Data.Items)
                {
                    allowed.Add(menu.Id);
                }
            }
            catch (ApiForbiddenException)
            {
                _logger.LogDebug("Role {RoleId} menu lookup forbidden for current user; skipping.", roleId);
            }
            catch (ApiUnauthorizedException)
            {
                _logger.LogDebug("Role {RoleId} menu lookup unauthorized for current user; skipping.", roleId);
            }
        }

        return allowed;
    }

    private static List<NavigationItem> Flatten(IReadOnlyList<MenuResponse> roots)
    {
        var list = new List<NavigationItem>();

        void Visit(MenuResponse node, Guid? parentId)
        {
            list.Add(new NavigationItem
            {
                Id = node.Id,
                ParentId = parentId,
                Name = string.IsNullOrEmpty(node.Name) ? node.NameKey : node.Name,
                Url = string.IsNullOrEmpty(node.Url) ? null : node.Url,
                Icon = string.IsNullOrEmpty(node.Icon) ? null : node.Icon,
                Order = node.Order,
                RequiredPermissions = node.RequiredPermissions
            });

            foreach (var child in node.Children)
            {
                Visit(child, node.Id);
            }
        }

        foreach (var root in roots)
        {
            Visit(root, null);
        }

        return list;
    }

    /// <summary>
    /// Keeps every menu the user is explicitly allowed to see plus every
    /// ancestor of those menus, so the parent containers remain navigable.
    /// </summary>
    private static IReadOnlyList<NavigationItem> FilterByAllowedAndKeepAncestors(
        IReadOnlyList<NavigationItem> all,
        HashSet<Guid> allowed,
        Func<string, bool> hasPermission)
    {
        if (allowed.Count == 0)
        {
            return Array.Empty<NavigationItem>();
        }

        var byId = all.ToDictionary(i => i.Id);
        var keep = new HashSet<Guid>();

        foreach (var item in all.Where(item => allowed.Contains(item.Id) && HasPermissionForMenu(item, hasPermission)))
        {
            keep.Add(item.Id);

            var parentId = item.ParentId;
            while (parentId is { } p && byId.TryGetValue(p, out var parent))
            {
                if (!keep.Add(parent.Id))
                {
                    break;
                }
                parentId = parent.ParentId;
            }
        }

        return all.Where(i => keep.Contains(i.Id)).ToArray();
    }

    private static bool HasPermissionForMenu(NavigationItem item, Func<string, bool> hasPermission)
    {
        if (item.RequiredPermissions.Count == 0)
        {
            return true;
        }

        return item.RequiredPermissions.All(hasPermission);
    }
}

