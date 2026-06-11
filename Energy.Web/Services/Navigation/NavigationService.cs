using System.Globalization;
using Energy.Localization;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.System;
using Energy.Web.Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Energy.Web.Services.Navigation;

public interface INavigationService
{
    Task<IReadOnlyList<NavigationItem>> GetMyNavigationAsync(CancellationToken ct = default);
}

/// <summary>
/// Loads the per-user menu tree from the API (already permission-filtered on
/// the server side) and always pins the per-user defaults (Dashboard +
/// Profile) on top so every authenticated user has somewhere to go even when
/// no menus are assigned to their roles.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);

    private static readonly HashSet<string> AlwaysVisibleMenuUrls =
        new(StringComparer.OrdinalIgnoreCase) { "/dashboard", "/profile" };

    private static readonly (string Key, string Url, string Icon, int Order)[] PinnedDefaults =
    {
        (LocalizationKeys.Menus.Dashboard, "/dashboard", "home", 1),
        (LocalizationKeys.Menus.Profile,   "/profile",   "user", 9999)
    };

    private readonly IMenuApiClient _menus;
    private readonly IMemoryCache _cache;
    private readonly IHttpContextAccessor _http;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<NavigationService> _logger;

    public NavigationService(
        IMenuApiClient menus,
        IMemoryCache cache,
        IHttpContextAccessor http,
        IStringLocalizer<SharedResource> localizer,
        ILogger<NavigationService> logger)
    {
        _menus = menus;
        _cache = cache;
        _http = http;
        _localizer = localizer;
        _logger = logger;
    }

    public async Task<IReadOnlyList<NavigationItem>> GetMyNavigationAsync(CancellationToken ct = default)
    {
        var user = _http.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return Array.Empty<NavigationItem>();
        }

        var cacheKey = $"energy.nav.{user.Identity?.Name ?? "anon"}.{CultureInfo.CurrentUICulture.Name}";
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<NavigationItem>? cached) && cached is not null)
        {
            return cached;
        }

        var items = new List<NavigationItem>();

        try
        {
            var response = await _menus.GetMyTreeAsync(ct);
            if (response.IsSuccess && response.Data is not null)
            {
                Flatten(response.Data, null, items);
            }
        }
        catch (ApiForbiddenException)
        {
            _logger.LogDebug("User is not allowed to read the menu tree; rendering pinned defaults only.");
        }
        catch (ApiUnauthorizedException)
        {
            _logger.LogDebug("User is no longer authorized for the menu tree.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Loading the menu tree failed; rendering pinned defaults only.");
        }

        AppendPinnedDefaults(items);

        var ordered = items
            .OrderBy(i => i.ParentId.HasValue)
            .ThenBy(i => i.Order)
            .ThenBy(i => i.Name)
            .ToArray();

        _cache.Set(cacheKey, (IReadOnlyList<NavigationItem>)ordered, CacheDuration);
        return ordered;
    }

    private void AppendPinnedDefaults(List<NavigationItem> sink)
    {
        var existingUrls = sink
            .Where(i => !string.IsNullOrEmpty(i.Url))
            .Select(i => i.Url!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var pinned in PinnedDefaults)
        {
            if (existingUrls.Contains(pinned.Url)) continue;
            sink.Add(new NavigationItem
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Name = _localizer.GetText(pinned.Key, pinned.Key),
                Url = pinned.Url,
                Icon = pinned.Icon,
                Order = pinned.Order
            });
        }
    }

    private static void Flatten(IReadOnlyList<MenuTreeNodeResponse> nodes, Guid? parentId, List<NavigationItem> sink)
    {
        foreach (var node in nodes)
        {
            sink.Add(new NavigationItem
            {
                Id = node.Id,
                ParentId = parentId,
                Name = node.Name,
                Url = node.Url,
                Icon = node.Icon,
                Order = node.DisplayOrder
            });
            if (node.Children.Count > 0) Flatten(node.Children, node.Id, sink);
        }
    }
}
