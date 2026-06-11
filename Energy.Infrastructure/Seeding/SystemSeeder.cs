using Energy.Application.Identity.Services;
using Energy.Application.Localization.Services;
using Energy.Application.System.Services;
using Energy.Domain.Identity;
using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Single entry point that brings the database to a fully usable state for
/// system administration: default permissions, default menu tree, the Admin
/// role/user with all permissions and menus linked, and the localization
/// overrides imported from the embedded .resx files.
///
/// Every step is idempotent so the seeder can safely run on every startup.
/// </summary>
public sealed class SystemSeeder
{
    private sealed record SeedRoleDefinition(
        string NameKey,
        string DescriptionKey,
        IReadOnlyList<string> PermissionCodes,
        IReadOnlyList<string> MenuUrls,
        SeedUserDefinition User);

    private sealed record SeedUserDefinition(
        string FirstNameKey,
        string LastNameKey,
        string UserName,
        string Email,
        string Password);

    /// <summary>
    /// One centralized access rule per real API endpoint (scope = API). Each rule
    /// is linked to the exact permission its controller action already enforces via
    /// <c>[Authorize(Policy = ...)]</c>, so enabling the rule mirrors the existing
    /// policy and never changes the effective authorization for any principal.
    /// This turns the Access Rules screen into a complete, self-documenting map of
    /// the API surface and how every endpoint relates to a permission.
    /// </summary>
    private sealed record SeedAccessRuleDefinition(
        string NameKey,
        string Path,
        string HttpMethod,
        string DescriptionKey,
        string PermissionCode);

    private const string ApiBase = "/api/v1";

    private static readonly SeedAccessRuleDefinition[] DefaultAccessRuleDefinitions =
    [
        // Home
        new(LocalizationKeys.AccessRulesSeed.Home.GetDashboardName, $"{ApiBase}/home/dashboard", "GET", LocalizationKeys.AccessRulesSeed.Home.GetDashboardDescription, HomePermissions.GetDashboard),

        // Users
        new(LocalizationKeys.AccessRulesSeed.User.GetUsersName, $"{ApiBase}/users", "GET", LocalizationKeys.AccessRulesSeed.User.GetUsersDescription, UserPermissions.GetUsers),
        new(LocalizationKeys.AccessRulesSeed.User.GetUserName, $"{ApiBase}/users/{{id}}", "GET", LocalizationKeys.AccessRulesSeed.User.GetUserDescription, UserPermissions.GetUser),
        new(LocalizationKeys.AccessRulesSeed.User.CreateUserName, $"{ApiBase}/users", "POST", LocalizationKeys.AccessRulesSeed.User.CreateUserDescription, UserPermissions.CreateUser),
        new(LocalizationKeys.AccessRulesSeed.User.UpdateUserName, $"{ApiBase}/users/{{id}}", "PUT", LocalizationKeys.AccessRulesSeed.User.UpdateUserDescription, UserPermissions.UpdateUser),
        new(LocalizationKeys.AccessRulesSeed.User.SetRolesName, $"{ApiBase}/users/{{id}}/roles", "PUT", LocalizationKeys.AccessRulesSeed.User.SetRolesDescription, UserPermissions.SetRoles),
        new(LocalizationKeys.AccessRulesSeed.User.UpdatePasswordName, $"{ApiBase}/users/{{id}}/password", "PUT", LocalizationKeys.AccessRulesSeed.User.UpdatePasswordDescription, UserPermissions.UpdatePassword),
        new(LocalizationKeys.AccessRulesSeed.User.DeleteUserName, $"{ApiBase}/users/{{id}}", "DELETE", LocalizationKeys.AccessRulesSeed.User.DeleteUserDescription, UserPermissions.DeleteUser),
        new(LocalizationKeys.AccessRulesSeed.User.GetAdminPermissionHealthName, $"{ApiBase}/users/admin-permissions/health", "GET", LocalizationKeys.AccessRulesSeed.User.GetAdminPermissionHealthDescription, UserPermissions.GetAdminPermissionHealth),

        // Permissions
        new(LocalizationKeys.AccessRulesSeed.Permission.GetPermissionsName, $"{ApiBase}/permissions", "GET", LocalizationKeys.AccessRulesSeed.Permission.GetPermissionsDescription, PermissionPermissions.GetPermissions),
        new(LocalizationKeys.AccessRulesSeed.Permission.GetPermissionName, $"{ApiBase}/permissions/{{id}}", "GET", LocalizationKeys.AccessRulesSeed.Permission.GetPermissionDescription, PermissionPermissions.GetPermission),
        new(LocalizationKeys.AccessRulesSeed.Permission.CreatePermissionName, $"{ApiBase}/permissions", "POST", LocalizationKeys.AccessRulesSeed.Permission.CreatePermissionDescription, PermissionPermissions.CreatePermission),
        new(LocalizationKeys.AccessRulesSeed.Permission.UpdatePermissionName, $"{ApiBase}/permissions/{{id}}", "PUT", LocalizationKeys.AccessRulesSeed.Permission.UpdatePermissionDescription, PermissionPermissions.UpdatePermission),
        new(LocalizationKeys.AccessRulesSeed.Permission.DeletePermissionName, $"{ApiBase}/permissions/{{id}}", "DELETE", LocalizationKeys.AccessRulesSeed.Permission.DeletePermissionDescription, PermissionPermissions.DeletePermission),

        // Roles
        new(LocalizationKeys.AccessRulesSeed.Role.GetRolesName, $"{ApiBase}/roles", "GET", LocalizationKeys.AccessRulesSeed.Role.GetRolesDescription, RolePermissions.GetRoles),
        new(LocalizationKeys.AccessRulesSeed.Role.GetRoleName, $"{ApiBase}/roles/{{id}}", "GET", LocalizationKeys.AccessRulesSeed.Role.GetRoleDescription, RolePermissions.GetRole),
        new(LocalizationKeys.AccessRulesSeed.Role.CreateRoleName, $"{ApiBase}/roles", "POST", LocalizationKeys.AccessRulesSeed.Role.CreateRoleDescription, RolePermissions.CreateRole),
        new(LocalizationKeys.AccessRulesSeed.Role.UpdateRoleName, $"{ApiBase}/roles/{{id}}", "PUT", LocalizationKeys.AccessRulesSeed.Role.UpdateRoleDescription, RolePermissions.UpdateRole),
        new(LocalizationKeys.AccessRulesSeed.Role.DeleteRoleName, $"{ApiBase}/roles/{{id}}", "DELETE", LocalizationKeys.AccessRulesSeed.Role.DeleteRoleDescription, RolePermissions.DeleteRole),
        new(LocalizationKeys.AccessRulesSeed.Role.GetRolePermissionsName, $"{ApiBase}/roles/{{id}}/permissions", "GET", LocalizationKeys.AccessRulesSeed.Role.GetRolePermissionsDescription, RolePermissions.GetRolePermissions),
        new(LocalizationKeys.AccessRulesSeed.Role.SetRolePermissionsName, $"{ApiBase}/roles/{{id}}/permissions", "PUT", LocalizationKeys.AccessRulesSeed.Role.SetRolePermissionsDescription, RolePermissions.SetRolePermissions),
        new(LocalizationKeys.AccessRulesSeed.Role.GetRoleMenusName, $"{ApiBase}/roles/{{id}}/menus", "GET", LocalizationKeys.AccessRulesSeed.Role.GetRoleMenusDescription, RolePermissions.GetRoleMenus),
        new(LocalizationKeys.AccessRulesSeed.Role.SetRoleMenusName, $"{ApiBase}/roles/{{id}}/menus", "PUT", LocalizationKeys.AccessRulesSeed.Role.SetRoleMenusDescription, RolePermissions.SetRoleMenus),

        // Menus
        new(LocalizationKeys.AccessRulesSeed.Menu.GetMenusName, $"{ApiBase}/menus", "GET", LocalizationKeys.AccessRulesSeed.Menu.GetMenusDescription, MenuPermissions.GetMenus),
        new(LocalizationKeys.AccessRulesSeed.Menu.GetMenuTreeName, $"{ApiBase}/menus/tree", "GET", LocalizationKeys.AccessRulesSeed.Menu.GetMenuTreeDescription, MenuPermissions.GetMenuTree),
        new(LocalizationKeys.AccessRulesSeed.Menu.GetMenuName, $"{ApiBase}/menus/{{id}}", "GET", LocalizationKeys.AccessRulesSeed.Menu.GetMenuDescription, MenuPermissions.GetMenu),
        new(LocalizationKeys.AccessRulesSeed.Menu.CreateMenuName, $"{ApiBase}/menus", "POST", LocalizationKeys.AccessRulesSeed.Menu.CreateMenuDescription, MenuPermissions.CreateMenu),
        new(LocalizationKeys.AccessRulesSeed.Menu.UpdateMenuName, $"{ApiBase}/menus/{{id}}", "PUT", LocalizationKeys.AccessRulesSeed.Menu.UpdateMenuDescription, MenuPermissions.UpdateMenu),
        new(LocalizationKeys.AccessRulesSeed.Menu.DeleteMenuName, $"{ApiBase}/menus/{{id}}", "DELETE", LocalizationKeys.AccessRulesSeed.Menu.DeleteMenuDescription, MenuPermissions.DeleteMenu),
        new(LocalizationKeys.AccessRulesSeed.Menu.GetMenuPermissionsName, $"{ApiBase}/menus/{{id}}/permissions", "GET", LocalizationKeys.AccessRulesSeed.Menu.GetMenuPermissionsDescription, MenuPermissions.GetMenuPermissions),
        new(LocalizationKeys.AccessRulesSeed.Menu.SetMenuPermissionsName, $"{ApiBase}/menus/{{id}}/permissions", "PUT", LocalizationKeys.AccessRulesSeed.Menu.SetMenuPermissionsDescription, MenuPermissions.SetMenuPermissions),

        // Localization
        new(LocalizationKeys.AccessRulesSeed.Localization.GetAllName, $"{ApiBase}/localization", "GET", LocalizationKeys.AccessRulesSeed.Localization.GetAllDescription, LocalizationPermissions.GetAll),
        new(LocalizationKeys.AccessRulesSeed.Localization.GetByKeyName, $"{ApiBase}/localization/{{key}}", "GET", LocalizationKeys.AccessRulesSeed.Localization.GetByKeyDescription, LocalizationPermissions.GetByKey),
        new(LocalizationKeys.AccessRulesSeed.Localization.UpsertName, $"{ApiBase}/localization", "POST", LocalizationKeys.AccessRulesSeed.Localization.UpsertDescription, LocalizationPermissions.Upsert),
        new(LocalizationKeys.AccessRulesSeed.Localization.DeleteName, $"{ApiBase}/localization/{{key}}", "DELETE", LocalizationKeys.AccessRulesSeed.Localization.DeleteDescription, LocalizationPermissions.Delete),

        // Access Rules
        new(LocalizationKeys.AccessRulesSeed.AccessRule.GetAccessRulesName, $"{ApiBase}/access-rules", "GET", LocalizationKeys.AccessRulesSeed.AccessRule.GetAccessRulesDescription, AccessRulePermissions.GetAccessRules),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.GetAccessRuleName, $"{ApiBase}/access-rules/{{id}}", "GET", LocalizationKeys.AccessRulesSeed.AccessRule.GetAccessRuleDescription, AccessRulePermissions.GetAccessRule),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.CreateAccessRuleName, $"{ApiBase}/access-rules", "POST", LocalizationKeys.AccessRulesSeed.AccessRule.CreateAccessRuleDescription, AccessRulePermissions.CreateAccessRule),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.UpdateAccessRuleName, $"{ApiBase}/access-rules/{{id}}", "PUT", LocalizationKeys.AccessRulesSeed.AccessRule.UpdateAccessRuleDescription, AccessRulePermissions.UpdateAccessRule),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.DeleteAccessRuleName, $"{ApiBase}/access-rules/{{id}}", "DELETE", LocalizationKeys.AccessRulesSeed.AccessRule.DeleteAccessRuleDescription, AccessRulePermissions.DeleteAccessRule),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.GetAccessRulePermissionsName, $"{ApiBase}/access-rules/{{id}}/permissions", "GET", LocalizationKeys.AccessRulesSeed.AccessRule.GetAccessRulePermissionsDescription, AccessRulePermissions.GetAccessRulePermissions),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.SetAccessRulePermissionsName, $"{ApiBase}/access-rules/{{id}}/permissions", "PUT", LocalizationKeys.AccessRulesSeed.AccessRule.SetAccessRulePermissionsDescription, AccessRulePermissions.SetAccessRulePermissions),
        new(LocalizationKeys.AccessRulesSeed.AccessRule.GetRequiredPermissionsName, $"{ApiBase}/access-rules/required-permissions", "GET", LocalizationKeys.AccessRulesSeed.AccessRule.GetRequiredPermissionsDescription, AccessRulePermissions.GetRequiredPermissions),
    ];

    /// <summary>
    /// Maps each leaf menu URL to the primary "view/list" permission it represents.
    /// Used to populate the MenuPermissions table so the per-menu permission screen
    /// is meaningful. Every seeded role linked to one of these menus already owns the
    /// mapped permission, so navigation visibility is unchanged. The "/system" parent
    /// is intentionally left unmapped so it stays visible as a pure container.
    /// </summary>
    private static readonly (string MenuUrl, string PermissionCode)[] DefaultMenuPermissionMap =
    [
        ("/dashboard", HomePermissions.GetDashboard),
        ("/system/users", UserPermissions.GetUsers),
        ("/system/roles", RolePermissions.GetRoles),
        ("/system/permissions", PermissionPermissions.GetPermissions),
        ("/system/menus", MenuPermissions.GetMenus),
        ("/system/localization", LocalizationPermissions.GetAll),
        ("/system/access-rules", AccessRulePermissions.GetAccessRules),
    ];

    private static readonly SeedRoleDefinition[] DefaultRoleDefinitions =
    [
        new(
            NameKey: LocalizationKeys.Roles.OperationsManagerDisplayName,
            DescriptionKey: LocalizationKeys.Roles.OperationsManagerDescription,
            PermissionCodes:
            [
                // Home
                HomePermissions.GetDashboard,
                // Users
                UserPermissions.GetUsers,
                UserPermissions.GetUser,
                UserPermissions.CreateUser,
                UserPermissions.UpdateUser,
                UserPermissions.SetRoles,
                UserPermissions.DeleteUser,
                // Permissions
                PermissionPermissions.GetPermissions,
                PermissionPermissions.GetPermission,
                PermissionPermissions.CreatePermission,
                PermissionPermissions.UpdatePermission,
                PermissionPermissions.DeletePermission,
                // Roles
                RolePermissions.GetRoles,
                RolePermissions.GetRole,
                RolePermissions.CreateRole,
                RolePermissions.UpdateRole,
                RolePermissions.DeleteRole,
                RolePermissions.GetRolePermissions,
                RolePermissions.SetRolePermissions,
                RolePermissions.GetRoleMenus,
                RolePermissions.SetRoleMenus,
                // Menus
                MenuPermissions.GetMenus,
                MenuPermissions.GetMenuTree,
                MenuPermissions.GetMenu,
                MenuPermissions.CreateMenu,
                MenuPermissions.UpdateMenu,
                MenuPermissions.DeleteMenu,
                MenuPermissions.GetMenuPermissions,
                MenuPermissions.SetMenuPermissions,
                // Access Rules
                AccessRulePermissions.GetAccessRules,
                AccessRulePermissions.GetAccessRule,
                AccessRulePermissions.CreateAccessRule,
                AccessRulePermissions.UpdateAccessRule,
                AccessRulePermissions.DeleteAccessRule,
                AccessRulePermissions.GetAccessRulePermissions,
                AccessRulePermissions.SetAccessRulePermissions,
                AccessRulePermissions.GetRequiredPermissions,
            ],
            MenuUrls: ["/dashboard", "/profile", "/system/users", "/system/roles", "/system/permissions", "/system/menus", "/system/access-rules"],
            User: new SeedUserDefinition(
                FirstNameKey: LocalizationKeys.Users.OperationsManagerFirstName,
                LastNameKey: LocalizationKeys.Users.OperationsManagerLastName,
                UserName: "ops.manager",
                Email: "ops.manager@energy.local",
                Password: "Manager123!")),
        new(
            NameKey: LocalizationKeys.Roles.LocalizationEditorDisplayName,
            DescriptionKey: LocalizationKeys.Roles.LocalizationEditorDescription,
            PermissionCodes:
            [
                HomePermissions.GetDashboard,
                LocalizationPermissions.GetAll,
                LocalizationPermissions.GetByKey,
                LocalizationPermissions.Upsert,
                LocalizationPermissions.Delete,
                // Menu tree read is required so the navigation drawer can be
                // populated for any signed-in user, regardless of the rest of
                // their permission footprint.
                MenuPermissions.GetMenuTree,
                RolePermissions.GetRoleMenus,
            ],
            MenuUrls: ["/dashboard", "/profile", "/system/localization"],
            User: new SeedUserDefinition(
                FirstNameKey: LocalizationKeys.Users.LocalizationEditorFirstName,
                LastNameKey: LocalizationKeys.Users.LocalizationEditorLastName,
                UserName: "localization.editor",
                Email: "localization.editor@energy.local",
                Password: "Editor123!")),
        new(
            NameKey: LocalizationKeys.Roles.ReadOnlyDisplayName,
            DescriptionKey: LocalizationKeys.Roles.ReadOnlyDescription,
            PermissionCodes:
            [
                HomePermissions.GetDashboard,
                UserPermissions.GetUsers,
                UserPermissions.GetUser,
                PermissionPermissions.GetPermissions,
                PermissionPermissions.GetPermission,
                RolePermissions.GetRoles,
                RolePermissions.GetRole,
                RolePermissions.GetRolePermissions,
                RolePermissions.GetRoleMenus,
                MenuPermissions.GetMenus,
                MenuPermissions.GetMenuTree,
                MenuPermissions.GetMenu,
                MenuPermissions.GetMenuPermissions,
                AccessRulePermissions.GetAccessRules,
                AccessRulePermissions.GetAccessRule,
                AccessRulePermissions.GetAccessRulePermissions,
                AccessRulePermissions.GetRequiredPermissions,
            ],
            MenuUrls: ["/dashboard", "/profile"],
            User: new SeedUserDefinition(
                FirstNameKey: LocalizationKeys.Users.ReadOnlyFirstName,
                LastNameKey: LocalizationKeys.Users.ReadOnlyLastName,
                UserName: "readonly.user",
                Email: "readonly.user@energy.local",
                Password: "Viewer123!"))
    ];

    private readonly AppDbContext _dbContext;
    private readonly IPermissionService _permissionService;
    private readonly IMenuService _menuService;
    private readonly IUserService _userService;
    private readonly ILocalizationService _localizationService;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<SystemSeeder> _logger;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SystemSeeder(
        AppDbContext dbContext,
        IPermissionService permissionService,
        IMenuService menuService,
        IUserService userService,
        ILocalizationService localizationService,
        IHostEnvironment environment,
        ILogger<SystemSeeder> logger,
        IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _permissionService = permissionService;
        _menuService = menuService;
        _userService = userService;
        _localizationService = localizationService;
        _environment = environment;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
        => await SeedAsync(additionalPermissionCodes: null, cancellationToken);

    public async Task SeedAsync(
        IReadOnlyCollection<string>? additionalPermissionCodes,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("System seeding started ({Environment}).", _environment.EnvironmentName);

        // 1) Permission catalog — required by both role assignment and JWT claims.
        var permissionResult = await _permissionService.SeedDefaultPermissionsAsync(cancellationToken);
        _logger.LogInformation("Permissions: {Added} added, {Updated} updated, total {Total}.",
            permissionResult.Added, permissionResult.Updated, permissionResult.Total);

        // 1.1) Auto-discovered permissions from [Authorize(Policy=...)] attributes.
        if (additionalPermissionCodes is { Count: > 0 })
        {
            var discoveredResult = await _permissionService.SeedPermissionCodesAsync(additionalPermissionCodes, cancellationToken);
            _logger.LogInformation(
                "Discovered permissions: {Added} added from {Scanned} policy code(s), total {Total}.",
                discoveredResult.Added, additionalPermissionCodes.Count, discoveredResult.Total);
        }

        // 2) Menu tree — depends on no other seed; cleans up legacy nodes too.
        var menuResult = await _menuService.SeedDefaultMenusAsync(cancellationToken);
        _logger.LogInformation("Menus: {Added} added, {Updated} updated, total {Total}.",
            menuResult.Added, menuResult.Updated, menuResult.Total);

        // 3) Admin user + Admin role + all permissions linked to Admin.
        var adminResult = await _userService.SeedAdminAsync(cancellationToken);
        _logger.LogInformation(
            "Admin: user '{Email}' (created={UserCreated}), role (created={RoleCreated}).",
            adminResult.Email, adminResult.UserCreated, adminResult.RoleCreated);

        // 4) Link every menu to the Admin role so the navigation is fully visible.
        await EnsureAllMenusLinkedToRoleAsync(adminResult.RoleId, cancellationToken);

        // 4.1) Seed additional user archetypes and role/menu/permission links.
        var identityCatalogResult = await SeedDefaultIdentityCatalogAsync(cancellationToken);
        _logger.LogInformation(
            "Identity catalog: {RolesAdded} role(s), {UsersAdded} user(s), {PermissionLinksAdded} permission link(s), {PermissionLinksRemoved} removed, {MenuLinksAdded} menu link(s), {MenuLinksRemoved} removed.",
            identityCatalogResult.RolesAdded,
            identityCatalogResult.UsersAdded,
            identityCatalogResult.PermissionLinksAdded,
            identityCatalogResult.PermissionLinksRemoved,
            identityCatalogResult.MenuLinksAdded,
            identityCatalogResult.MenuLinksRemoved);

        // 4.2) Centralized access rules (scope=API) mirroring every endpoint's policy,
        //      each linked to the permission it already enforces.
        var accessRuleResult = await SeedDefaultAccessRulesAsync(cancellationToken);
        _logger.LogInformation(
            "Access rules: {RulesAdded} added, {RulesExisting} existing, {PermissionLinksAdded} permission link(s) added.",
            accessRuleResult.RulesAdded, accessRuleResult.RulesExisting, accessRuleResult.PermissionLinksAdded);

        // 4.3) Menu → permission links so the per-menu permission catalog is populated.
        var menuPermissionLinks = await SeedDefaultMenuPermissionsAsync(cancellationToken);
        _logger.LogInformation("Menu permissions: {Added} link(s) added.", menuPermissionLinks);

        // 5) Mirror the .resx fallback values into the LocalizationEntries table
        //    so the DB-first localizer has a complete snapshot to serve from.
        var localizationResult = await _localizationService.ImportFromResxAsync(cancellationToken);
        _logger.LogInformation(
            "Localization import: {Added} added, {Updated} updated, total {Total}.",
            localizationResult.Added, localizationResult.Updated, localizationResult.Total);

        _logger.LogInformation("System seeding completed.");
    }

    private sealed record IdentityCatalogSeedResult(
        int RolesAdded,
        int UsersAdded,
        int PermissionLinksAdded,
        int PermissionLinksRemoved,
        int MenuLinksAdded,
        int MenuLinksRemoved);

    private async Task<IdentityCatalogSeedResult> SeedDefaultIdentityCatalogAsync(CancellationToken cancellationToken)
    {
        var rolesAdded = 0;
        var usersAdded = 0;
        var permissionLinksAdded = 0;
        var permissionLinksRemoved = 0;
        var menuLinksAdded = 0;
        var menuLinksRemoved = 0;

        var permissionsByCode = await _dbContext.Permissions
            .AsNoTracking()
            .ToDictionaryAsync(item => item.Code, item => item.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var menusByUrl = await _dbContext.Menus
            .AsNoTracking()
            .ToDictionaryAsync(item => item.Url, item => item.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var definition in DefaultRoleDefinitions)
        {
            var role = await EnsureRoleAsync(definition, cancellationToken);
            if (role.Created)
            {
                rolesAdded += 1;
            }

            var desiredPermissionIds = definition.PermissionCodes
                .Where(code => permissionsByCode.ContainsKey(code))
                .Select(code => permissionsByCode[code])
                .Distinct()
                .ToArray();

            var desiredMenuIds = definition.MenuUrls
                .Where(url => menusByUrl.ContainsKey(url))
                .Select(url => menusByUrl[url])
                .Distinct()
                .ToArray();

            var (addedPermissions, removedPermissions) = await SyncRolePermissionsAsync(role.RoleId, desiredPermissionIds, cancellationToken);
            var (addedMenus, removedMenus) = await SyncRoleMenusAsync(role.RoleId, desiredMenuIds, cancellationToken);

            permissionLinksAdded += addedPermissions;
            permissionLinksRemoved += removedPermissions;
            menuLinksAdded += addedMenus;
            menuLinksRemoved += removedMenus;

            var createdUser = await EnsureUserForRoleAsync(definition.User, role.RoleId, cancellationToken);
            if (createdUser)
            {
                usersAdded += 1;
            }
        }

        return new IdentityCatalogSeedResult(
            RolesAdded: rolesAdded,
            UsersAdded: usersAdded,
            PermissionLinksAdded: permissionLinksAdded,
            PermissionLinksRemoved: permissionLinksRemoved,
            MenuLinksAdded: menuLinksAdded,
            MenuLinksRemoved: menuLinksRemoved);
    }

    private async Task<(Guid RoleId, bool Created)> EnsureRoleAsync(SeedRoleDefinition definition, CancellationToken cancellationToken)
    {
        var displayName = _localizer.GetText(definition.NameKey, definition.NameKey);
        var description = _localizer.GetText(definition.DescriptionKey, definition.DescriptionKey);

        var normalizedName = displayName.Trim().ToUpperInvariant();
        var role = await _dbContext.Roles.FirstOrDefaultAsync(item => item.NormalizedName == normalizedName, cancellationToken);

        if (role is null)
        {
            role = new Role
            {
                Id = Guid.NewGuid(),
                Name = displayName.Trim(),
                NormalizedName = normalizedName,
                Description = description,
                ConcurrencyStamp = Guid.NewGuid().ToString("N")
            };

            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return (role.Id, true);
        }

        var changed = false;

        if (!string.Equals(role.Description, description, StringComparison.Ordinal))
        {
            role.Description = description;
            changed = true;
        }

        if (!string.Equals(role.Name, displayName, StringComparison.Ordinal))
        {
            role.Name = displayName;
            changed = true;
        }

        if (changed)
        {
            role.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return (role.Id, false);
    }

    private async Task<(int Added, int Removed)> SyncRolePermissionsAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> desiredPermissionIds,
        CancellationToken cancellationToken)
    {
        var existingLinks = await _dbContext.RolePermissions
            .Where(item => item.RoleId == roleId)
            .ToListAsync(cancellationToken);

        var existingIds = existingLinks.Select(item => item.PermissionId).ToHashSet();
        var desiredIds = desiredPermissionIds.ToHashSet();

        var toRemove = existingLinks.Where(item => !desiredIds.Contains(item.PermissionId)).ToList();
        var toAdd = desiredIds.Where(id => !existingIds.Contains(id))
            .Select(permissionId => new RolePermission { RoleId = roleId, PermissionId = permissionId })
            .ToList();

        if (toRemove.Count > 0)
        {
            _dbContext.RolePermissions.RemoveRange(toRemove);
        }

        if (toAdd.Count > 0)
        {
            await _dbContext.RolePermissions.AddRangeAsync(toAdd, cancellationToken);
        }

        if (toRemove.Count > 0 || toAdd.Count > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return (toAdd.Count, toRemove.Count);
    }

    private async Task<(int Added, int Removed)> SyncRoleMenusAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> desiredMenuIds,
        CancellationToken cancellationToken)
    {
        var existingLinks = await _dbContext.RoleMenus
            .Where(item => item.RoleId == roleId)
            .ToListAsync(cancellationToken);

        var existingIds = existingLinks.Select(item => item.MenuId).ToHashSet();
        var desiredIds = desiredMenuIds.ToHashSet();

        var toRemove = existingLinks.Where(item => !desiredIds.Contains(item.MenuId)).ToList();
        var toAdd = desiredIds.Where(id => !existingIds.Contains(id))
            .Select(menuId => new RoleMenu { RoleId = roleId, MenuId = menuId })
            .ToList();

        if (toRemove.Count > 0)
        {
            _dbContext.RoleMenus.RemoveRange(toRemove);
        }

        if (toAdd.Count > 0)
        {
            await _dbContext.RoleMenus.AddRangeAsync(toAdd, cancellationToken);
        }

        if (toRemove.Count > 0 || toAdd.Count > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return (toAdd.Count, toRemove.Count);
    }

    private async Task<bool> EnsureUserForRoleAsync(SeedUserDefinition definition, Guid roleId, CancellationToken cancellationToken)
    {
        var normalizedEmail = definition.Email.Trim().ToUpperInvariant();
        var existingUser = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail, cancellationToken);

        if (existingUser is null)
        {
            await _userService.CreateUserAsync(
                new CreateUserRequest
                {
                    FirstName = _localizer.GetText(definition.FirstNameKey, definition.FirstNameKey),
                    LastName = _localizer.GetText(definition.LastNameKey, definition.LastNameKey),
                    UserName = definition.UserName,
                    Email = definition.Email,
                    Password = definition.Password,
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    RoleIds = [roleId]
                },
                cancellationToken);

            return true;
        }

        var hasRole = await _dbContext.UserRoles.AnyAsync(
            item => item.UserId == existingUser.Id && item.RoleId == roleId,
            cancellationToken);

        if (!hasRole)
        {
            _dbContext.UserRoles.Add(new UserRole { UserId = existingUser.Id, RoleId = roleId });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return false;
    }

    private sealed record AccessRuleSeedResult(int RulesAdded, int RulesExisting, int PermissionLinksAdded);

    private async Task<AccessRuleSeedResult> SeedDefaultAccessRulesAsync(CancellationToken cancellationToken)
    {
        var rulesAdded = 0;
        var rulesExisting = 0;
        var permissionLinksAdded = 0;

        var permissionsByCode = await _dbContext.Permissions
            .AsNoTracking()
            .ToDictionaryAsync(item => item.Code, item => item.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var definition in DefaultAccessRuleDefinitions)
        {
            const string scope = "API";
            var path = definition.Path.Trim();
            var method = definition.HttpMethod.Trim().ToUpperInvariant();
            var name = _localizer.GetText(definition.NameKey, definition.NameKey);
            var description = _localizer.GetText(definition.DescriptionKey, definition.DescriptionKey);

            var rule = await _dbContext.AccessRules.FirstOrDefaultAsync(
                item => item.Scope == scope && item.Path == path && item.HttpMethod == method,
                cancellationToken);

            if (rule is null)
            {
                rule = new AccessRule
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Scope = scope,
                    Path = path,
                    HttpMethod = method,
                    Description = description,
                    IsEnabled = true,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.AccessRules.Add(rule);
                await _dbContext.SaveChangesAsync(cancellationToken);
                rulesAdded += 1;
            }
            else
            {
                rulesExisting += 1;
            }

            // Link the rule to the exact permission its controller action enforces.
            // Add-only (non-destructive) so manual permission tweaks are preserved.
            if (permissionsByCode.TryGetValue(definition.PermissionCode, out var permissionId))
            {
                var linkExists = await _dbContext.AccessRulePermissions.AnyAsync(
                    link => link.AccessRuleId == rule.Id && link.PermissionId == permissionId,
                    cancellationToken);

                if (!linkExists)
                {
                    _dbContext.AccessRulePermissions.Add(new AccessRulePermission
                    {
                        AccessRuleId = rule.Id,
                        PermissionId = permissionId
                    });
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    permissionLinksAdded += 1;
                }
            }
        }

        return new AccessRuleSeedResult(rulesAdded, rulesExisting, permissionLinksAdded);
    }

    private async Task<int> SeedDefaultMenuPermissionsAsync(CancellationToken cancellationToken)
    {
        var added = 0;

        var permissionsByCode = await _dbContext.Permissions
            .AsNoTracking()
            .ToDictionaryAsync(item => item.Code, item => item.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var menusByUrl = await _dbContext.Menus
            .AsNoTracking()
            .ToDictionaryAsync(item => item.Url, item => item.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var (menuUrl, permissionCode) in DefaultMenuPermissionMap)
        {
            if (!menusByUrl.TryGetValue(menuUrl, out var menuId) ||
                !permissionsByCode.TryGetValue(permissionCode, out var permissionId))
            {
                continue;
            }

            var linkExists = await _dbContext.MenuPermissions.AnyAsync(
                link => link.MenuId == menuId && link.PermissionId == permissionId,
                cancellationToken);

            if (!linkExists)
            {
                _dbContext.MenuPermissions.Add(new MenuPermission
                {
                    MenuId = menuId,
                    PermissionId = permissionId
                });
                await _dbContext.SaveChangesAsync(cancellationToken);
                added += 1;
            }
        }

        return added;
    }

    /// <summary>
    /// Ensures that every menu currently in the database is reachable by the
    /// supplied role. Used so the freshly-seeded Admin role automatically owns
    /// the entire menu tree without manual configuration.
    /// </summary>
    private async Task EnsureAllMenusLinkedToRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var menuIds = await _dbContext.Menus
            .Select(menu => menu.Id)
            .ToListAsync(cancellationToken);

        if (menuIds.Count == 0)
        {
            return;
        }

        var existingLinks = await _dbContext.RoleMenus
            .Where(link => link.RoleId == roleId)
            .Select(link => link.MenuId)
            .ToListAsync(cancellationToken);

        var missingIds = menuIds.Except(existingLinks).ToList();
        if (missingIds.Count == 0)
        {
            return;
        }

        await _dbContext.RoleMenus.AddRangeAsync(
            missingIds.Select(menuId => new RoleMenu { RoleId = roleId, MenuId = menuId }),
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Linked {Count} menu(s) to the Admin role.", missingIds.Count);
    }
}

