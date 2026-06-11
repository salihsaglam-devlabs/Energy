using Energy.Application.Common.Exceptions;
using Energy.Application.System.Services;
using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.System.Services;

public sealed class MenuService : IMenuService
{
    /// <summary>
    /// Seed catalog. <c>NameKey</c> stores a localization key resolved at read
    /// time by <see cref="IStringLocalizer{SharedResource}"/>. <c>Url</c> is the
    /// uniqueness anchor — changing it produces a new row on the next seed.
    /// </summary>
    private sealed record MenuSeedNode(
        string NameKey,
        string Url,
        string Icon,
        int Order,
        MenuSeedNode[] Children);

    private static readonly MenuSeedNode[] DefaultMenuTree =
    [
        // Top-level: dashboard (always visible after login).
        new(LocalizationKeys.Menus.Dashboard, "/dashboard", "home", 1, []),

        // User's own profile — always available for every authenticated user.
        new(LocalizationKeys.Menus.Profile, "/profile", "user", 2, []),

        // Top-level: system management container — one child per real API endpoint group.
        new(LocalizationKeys.Menus.System, "/system", "preferences", 10,
        [
            new(LocalizationKeys.Menus.SystemUsers,        "/system/users",        "group",      11, []),
            new(LocalizationKeys.Menus.SystemRoles,        "/system/roles",        "accountbox", 12, []),
            new(LocalizationKeys.Menus.SystemPermissions,  "/system/permissions",  "key",        13, []),
            new(LocalizationKeys.Menus.SystemMenus,        "/system/menus",        "menu",       14, []),
            new(LocalizationKeys.Menus.SystemLocalization, "/system/localization", "globe",      15, []),
            new("Menus.System.AccessRules",               "/system/access-rules", "lock",       16, [])
        ])
    ];

    /// <summary>
    /// URLs of menu nodes that were emitted by previous versions of the seed
    /// catalog but are no longer required (no real screen behind them). They
    /// are removed during <see cref="SeedDefaultMenusAsync"/> together with
    /// any role-menu links that still reference them.
    /// </summary>
    private static readonly string[] ObsoleteSeededUrls =
    [
        "/identity",
        "/identity/profile",
        "/identity/sessions",
        "/inventory",
        "/inventory/projects",
        "/inventory/warehouses",
        "/reports",
        "/reports/projects",
        "/reports/warehouses"
    ];

    private readonly AppDbContext _dbContext;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public MenuService(AppDbContext dbContext, IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    public async Task<IReadOnlyList<MenuResponse>> GetMenusAsync(CancellationToken cancellationToken = default)
    {
        var menus = await _dbContext.Menus
            .AsNoTracking()
            .OrderBy(m => m.Order)
            .ThenBy(m => m.Name)
            .ToListAsync(cancellationToken);

        var permissionLookup = await BuildPermissionLookupAsync(
            menus.Select(menu => menu.Id).ToArray(),
            cancellationToken);

        return menus.Select(menu => MapFlat(menu, permissionLookup)).ToList();
    }

    public async Task<IReadOnlyList<MenuResponse>> GetMenuTreeAsync(CancellationToken cancellationToken = default)
    {
        var menus = await _dbContext.Menus
            .AsNoTracking()
            .OrderBy(m => m.Order)
            .ThenBy(m => m.Name)
            .ToListAsync(cancellationToken);

        var permissionLookup = await BuildPermissionLookupAsync(
            menus.Select(menu => menu.Id).ToArray(),
            cancellationToken);

        // Group children by parent id once for O(N) tree assembly.
        var childrenByParent = menus
            .Where(m => m.ParentId.HasValue)
            .GroupBy(m => m.ParentId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(m => m.Order).ToList());

        IReadOnlyList<MenuResponse> BuildLevel(IEnumerable<Menu> nodes) =>
            nodes
                .OrderBy(n => n.Order)
                .ThenBy(n => n.Name)
                .Select(n => MapWithChildren(n, childrenByParent, permissionLookup))
                .ToList();

        var roots = menus.Where(m => !m.ParentId.HasValue);
        return BuildLevel(roots);
    }

    public async Task<MenuResponse> GetMenuByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await _dbContext.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        return menu is null
            ? throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.MenuNotFound, "Menu '{0}' was not found."),
                id))
            : MapFlat(menu, await BuildPermissionLookupAsync([id], cancellationToken));
    }

    public async Task<MenuResponse> CreateMenuAsync(CreateMenuRequest request, CancellationToken cancellationToken = default)
    {
        var url = request.Url.Trim();
        if (await _dbContext.Menus.AnyAsync(m => m.Url == url, cancellationToken))
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.MenuUrlAlreadyExists, "Menu with url '{0}' already exists."),
                url));
        }

        if (request.ParentId.HasValue &&
            !await _dbContext.Menus.AnyAsync(m => m.Id == request.ParentId.Value, cancellationToken))
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.ParentMenuNotFound, "Parent menu '{0}' was not found."),
                request.ParentId));
        }

        var menu = new Menu
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Url = url,
            Icon = request.Icon.Trim(),
            Order = request.Order,
            ParentId = request.ParentId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Menus.Add(menu);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new MenuResponse
        {
            Id = menu.Id,
            Name = ResolveDisplayName(menu.Name),
            NameKey = menu.Name,
            Url = menu.Url,
            Icon = menu.Icon,
            Order = menu.Order,
            ParentId = menu.ParentId,
            RequiredPermissions = Array.Empty<string>()
        };
    }

    public async Task<MenuResponse> UpdateMenuAsync(Guid id, UpdateMenuRequest request, CancellationToken cancellationToken = default)
    {
        var menu = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.MenuNotFound, "Menu '{0}' was not found."),
                       id));

        var url = request.Url.Trim();
        if (await _dbContext.Menus.AnyAsync(m => m.Id != id && m.Url == url, cancellationToken))
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.MenuUrlAlreadyExists, "Menu with url '{0}' already exists."),
                url));
        }

        if (request.ParentId.HasValue)
        {
            if (request.ParentId.Value == id)
            {
                throw new ConflictException(_localizer.GetText(
                    LocalizationKeys.Messages.MenuSelfParent,
                    "A menu cannot be its own parent."));
            }

            if (!await _dbContext.Menus.AnyAsync(m => m.Id == request.ParentId.Value, cancellationToken))
            {
                throw new NotFoundException(string.Format(
                    _localizer.GetText(LocalizationKeys.Messages.ParentMenuNotFound, "Parent menu '{0}' was not found."),
                    request.ParentId));
            }
        }

        menu.Name = request.Name.Trim();
        menu.Url = url;
        menu.Icon = request.Icon.Trim();
        menu.Order = request.Order;
        menu.ParentId = request.ParentId;
        menu.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new MenuResponse
        {
            Id = menu.Id,
            Name = ResolveDisplayName(menu.Name),
            NameKey = menu.Name,
            Url = menu.Url,
            Icon = menu.Icon,
            Order = menu.Order,
            ParentId = menu.ParentId,
            RequiredPermissions = Array.Empty<string>()
        };
    }

    public async Task DeleteMenuAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.MenuNotFound, "Menu '{0}' was not found."),
                       id));

        // Cascade-delete children to avoid orphans.
        var descendants = await CollectDescendantsAsync(id, cancellationToken);
        if (descendants.Count > 0)
        {
            _dbContext.Menus.RemoveRange(descendants);
        }

        _dbContext.Menus.Remove(menu);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PermissionResponse>> GetMenuPermissionsAsync(Guid menuId, CancellationToken cancellationToken = default)
    {
        await EnsureMenuExistsAsync(menuId, cancellationToken);

        return await _dbContext.MenuPermissions
            .AsNoTracking()
            .Where(link => link.MenuId == menuId)
            .Join(_dbContext.Permissions.AsNoTracking(),
                link => link.PermissionId,
                permission => permission.Id,
                (link, permission) => new PermissionResponse
                {
                    Id = permission.Id,
                    Code = permission.Code,
                    Name = permission.Name
                })
            .OrderBy(permission => permission.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PermissionResponse>> SetMenuPermissionsAsync(
        Guid menuId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default)
    {
        await EnsureMenuExistsAsync(menuId, cancellationToken);

        var distinctIds = permissionIds?.Distinct().ToArray() ?? Array.Empty<Guid>();
        if (distinctIds.Length > 0)
        {
            var existingCount = await _dbContext.Permissions
                .AsNoTracking()
                .CountAsync(permission => distinctIds.Contains(permission.Id), cancellationToken);

            if (existingCount != distinctIds.Length)
            {
                throw new NotFoundException(_localizer.GetText(
                    LocalizationKeys.Messages.PermissionsNotFound,
                    "One or more permissions were not found."));
            }
        }

        var existingLinks = await _dbContext.MenuPermissions
            .Where(link => link.MenuId == menuId)
            .ToListAsync(cancellationToken);

        var toRemove = existingLinks.Where(link => !distinctIds.Contains(link.PermissionId)).ToList();
        var existingIds = existingLinks.Select(link => link.PermissionId).ToHashSet();
        var toAdd = distinctIds.Where(id => !existingIds.Contains(id))
            .Select(id => new MenuPermission { MenuId = menuId, PermissionId = id });

        if (toRemove.Count > 0)
        {
            _dbContext.MenuPermissions.RemoveRange(toRemove);
        }

        await _dbContext.MenuPermissions.AddRangeAsync(toAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetMenuPermissionsAsync(menuId, cancellationToken);
    }

    public async Task<SeedResultResponse> SeedDefaultMenusAsync(CancellationToken cancellationToken = default)
    {
        var added = 0;
        var updated = 0;

        // Two-pass seed: first ensure every node exists (parents before children),
        // then link ParentId in a second pass once Ids are known.
        var nodeByUrl = new Dictionary<string, Menu>(StringComparer.OrdinalIgnoreCase);

        async Task EnsureNodeAsync(MenuSeedNode node, Menu? parent)
        {
            var existing = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Url == node.Url, cancellationToken);
            if (existing is null)
            {
                existing = new Menu
                {
                    Id = Guid.NewGuid(),
                    Name = node.NameKey,
                    Url = node.Url,
                    Icon = node.Icon,
                    Order = node.Order,
                    ParentId = parent?.Id,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Menus.Add(existing);
                added++;
            }
            else
            {
                var changed =
                    existing.Name != node.NameKey ||
                    existing.Icon != node.Icon ||
                    existing.Order != node.Order ||
                    existing.ParentId != parent?.Id;

                if (changed)
                {
                    existing.Name = node.NameKey;
                    existing.Icon = node.Icon;
                    existing.Order = node.Order;
                    existing.ParentId = parent?.Id;
                    existing.UpdatedAt = DateTime.UtcNow;
                    updated++;
                }
            }

            nodeByUrl[node.Url] = existing;

            // Save now so children can reference the (possibly new) parent Id.
            await _dbContext.SaveChangesAsync(cancellationToken);

            foreach (var child in node.Children)
            {
                await EnsureNodeAsync(child, existing);
            }
        }

        foreach (var root in DefaultMenuTree)
        {
            await EnsureNodeAsync(root, parent: null);
        }

        // Sweep up any legacy seeded menus that no longer exist in the catalog.
        var obsoleteMenus = await _dbContext.Menus
            .Where(menu => ObsoleteSeededUrls.Contains(menu.Url))
            .ToListAsync(cancellationToken);

        if (obsoleteMenus.Count > 0)
        {
            var obsoleteIds = obsoleteMenus.Select(menu => menu.Id).ToList();
            var obsoleteRoleLinks = await _dbContext.RoleMenus
                .Where(link => obsoleteIds.Contains(link.MenuId))
                .ToListAsync(cancellationToken);

            if (obsoleteRoleLinks.Count > 0)
            {
                _dbContext.RoleMenus.RemoveRange(obsoleteRoleLinks);
            }

            _dbContext.Menus.RemoveRange(obsoleteMenus);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var total = await _dbContext.Menus.AsNoTracking().CountAsync(cancellationToken);
        return new SeedResultResponse { Added = added, Updated = updated, Total = total };
    }

    private async Task<List<Menu>> CollectDescendantsAsync(Guid rootId, CancellationToken cancellationToken)
    {
        var all = await _dbContext.Menus.Where(m => m.ParentId != null).ToListAsync(cancellationToken);
        var lookup = all.GroupBy(m => m.ParentId!.Value).ToDictionary(g => g.Key, g => g.ToList());

        var stack = new Stack<Guid>();
        stack.Push(rootId);
        var result = new List<Menu>();

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (!lookup.TryGetValue(current, out var children))
            {
                continue;
            }

            foreach (var child in children)
            {
                result.Add(child);
                stack.Push(child.Id);
            }
        }

        return result;
    }

    private MenuResponse MapFlat(Menu menu, IReadOnlyDictionary<Guid, IReadOnlyList<string>>? permissionLookup = null) => new()
    {
        Id = menu.Id,
        Name = ResolveDisplayName(menu.Name),
        NameKey = menu.Name,
        Url = menu.Url,
        Icon = menu.Icon,
        Order = menu.Order,
        ParentId = menu.ParentId,
        RequiredPermissions = permissionLookup?.GetValueOrDefault(menu.Id, Array.Empty<string>()) ?? Array.Empty<string>()
    };

    private MenuResponse MapWithChildren(
        Menu menu,
        IReadOnlyDictionary<Guid, List<Menu>> childrenByParent,
        IReadOnlyDictionary<Guid, IReadOnlyList<string>> permissionLookup)
    {
        var children = childrenByParent.TryGetValue(menu.Id, out var list)
            ? list.Select(c => MapWithChildren(c, childrenByParent, permissionLookup)).ToList()
            : new List<MenuResponse>();

        return new MenuResponse
        {
            Id = menu.Id,
            Name = ResolveDisplayName(menu.Name),
            NameKey = menu.Name,
            Url = menu.Url,
            Icon = menu.Icon,
            Order = menu.Order,
            ParentId = menu.ParentId,
            RequiredPermissions = permissionLookup.GetValueOrDefault(menu.Id, Array.Empty<string>()),
            Children = children
        };
    }

    private async Task<Dictionary<Guid, IReadOnlyList<string>>> BuildPermissionLookupAsync(
        IReadOnlyCollection<Guid> menuIds,
        CancellationToken cancellationToken)
    {
        if (menuIds.Count == 0)
        {
            return new Dictionary<Guid, IReadOnlyList<string>>();
        }

        var pairs = await _dbContext.MenuPermissions
            .AsNoTracking()
            .Where(link => menuIds.Contains(link.MenuId))
            .Join(_dbContext.Permissions.AsNoTracking(),
                link => link.PermissionId,
                permission => permission.Id,
                (link, permission) => new { link.MenuId, permission.Code })
            .ToListAsync(cancellationToken);

        return pairs
            .GroupBy(pair => pair.MenuId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<string>)group
                    .Select(item => item.Code)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(code => code, StringComparer.OrdinalIgnoreCase)
                    .ToArray());
    }

    private async Task EnsureMenuExistsAsync(Guid menuId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Menus.AsNoTracking().AnyAsync(menu => menu.Id == menuId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.MenuNotFound, "Menu '{0}' was not found."),
                menuId));
        }
    }

    /// <summary>
    /// Resolves the stored name against the localization resource for the
    /// current request culture. If the value is not a known key (e.g. legacy
    /// data created via the admin UI), the original string is returned as-is.
    /// </summary>
    private string ResolveDisplayName(string nameOrKey)
    {
        if (string.IsNullOrWhiteSpace(nameOrKey))
        {
            return string.Empty;
        }

        var localized = _localizer[nameOrKey];
        return localized.ResourceNotFound ? nameOrKey : localized.Value;
    }
}
