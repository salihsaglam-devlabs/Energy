using Energy.Application.Identity.Services;
using Energy.Application.Localization.Services;
using Energy.Domain.Identity;
using Energy.Domain.System;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.System.Services;
using Energy.Localization;
using Energy.Shared.Identity;
using Energy.Shared.Identity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Idempotent startup seeder. Brings the database to a fully usable state:
/// permission catalog, SuperAdmin + admin user, baseline menu tree, API
/// endpoint catalog with default permission mapping, localization import,
/// and a curated catalog of sample roles + demo users covering the common
/// usage patterns the new architecture supports. Every step is safe to re-run.
/// </summary>
public sealed class SystemSeeder
{
    /// <summary>
    /// Reference catalog of user archetypes. Each entry maps a role to the
    /// exact set of permission codes it owns and the demo user that wears it.
    /// Admins can copy these as templates from the Roles screen.
    /// </summary>
    private sealed record SampleRole(
        string RoleName,
        string RoleDescription,
        IReadOnlyList<string> PermissionCodes,
        SampleUser? DemoUser);

    private sealed record SampleUser(
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password);

    /// <summary>
    /// Built-in role templates. SuperAdmin is handled separately because it
    /// bypasses permission checks; it does not appear here.
    /// </summary>
    private static readonly IReadOnlyList<SampleRole> SampleRoles =
    [
        // ---------------- IT / Platform administration ----------------
        // Permission-based full administrator: owns EVERY catalog permission.
        // Distinct from SuperAdmin (which bypasses checks and is system-locked);
        // SystemAdmin is fully manageable from the Roles screen yet covers every
        // module — so the catalog has no orphaned permission left unassigned.
        new(
            RoleName: "SystemAdmin",
            RoleDescription: LocalizationKeys.RoleSeed.SystemAdminDescription,
            PermissionCodes: [.. PermissionCatalog.All.Select(p => p.Code)],
            DemoUser: new SampleUser("system.admin", "system.admin@energy.local", "Selin", "Aydın", "SysAdmin123!")),

        // ---------------- Operational management (no security ops) ----------------
        new(
            RoleName: "OperationsManager",
            RoleDescription: LocalizationKeys.RoleSeed.OperationsManagerDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.UserReadAll, PermissionCatalog.UserRead,
                PermissionCatalog.UserCreate, PermissionCatalog.UserUpdate,
                PermissionCatalog.RoleReadAll, PermissionCatalog.RoleRead,
                PermissionCatalog.MenuReadAll, PermissionCatalog.MenuRead,
                PermissionCatalog.MenuCreate, PermissionCatalog.MenuUpdate, PermissionCatalog.MenuDelete,
                PermissionCatalog.LogReadAll, PermissionCatalog.LogRead,
            ],
            DemoUser: new SampleUser("ops.manager", "ops.manager@energy.local", "Mert", "Yıldız", "OpsMgr123!")),

        // ---------------- Security / compliance ----------------
        new(
            RoleName: "SecurityAuditor",
            RoleDescription: LocalizationKeys.RoleSeed.SecurityAuditorDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.UserReadAll, PermissionCatalog.UserRead,
                PermissionCatalog.RoleReadAll, PermissionCatalog.RoleRead,
                PermissionCatalog.PermissionReadAll, PermissionCatalog.PermissionRead,
                PermissionCatalog.ApiAccessReadAll, PermissionCatalog.ApiAccessRead,
                PermissionCatalog.MenuReadAll, PermissionCatalog.MenuRead,
                PermissionCatalog.LogReadAll, PermissionCatalog.LogRead,
            ],
            DemoUser: new SampleUser("security.auditor", "security.auditor@energy.local", "Deniz", "Kaya", "Auditor123!")),

        // ---------------- Translation / content ----------------
        new(
            RoleName: "LocalizationEditor",
            RoleDescription: LocalizationKeys.RoleSeed.LocalizationEditorDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.LocalizationReadAll, PermissionCatalog.LocalizationRead,
                PermissionCatalog.LocalizationCreate, PermissionCatalog.LocalizationUpdate, PermissionCatalog.LocalizationDelete,
            ],
            DemoUser: new SampleUser("localization.editor", "localization.editor@energy.local", "Elif", "Demir", "Editor123!")),

        // ---------------- Reporting / view-only ----------------
        new(
            RoleName: "ReadOnlyViewer",
            RoleDescription: LocalizationKeys.RoleSeed.ReadOnlyViewerDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.UserReadAll, PermissionCatalog.UserRead,
                PermissionCatalog.RoleReadAll, PermissionCatalog.RoleRead,
                PermissionCatalog.PermissionReadAll, PermissionCatalog.PermissionRead,
                PermissionCatalog.MenuReadAll, PermissionCatalog.MenuRead,
                PermissionCatalog.ApiAccessReadAll, PermissionCatalog.ApiAccessRead,
                PermissionCatalog.LocalizationReadAll, PermissionCatalog.LocalizationRead,
                PermissionCatalog.LogReadAll, PermissionCatalog.LogRead,
            ],
            DemoUser: new SampleUser("readonly.viewer", "readonly.viewer@energy.local", "Ayşe", "Çelik", "Viewer123!")),

        // ---------------- Minimum baseline employee ----------------
        new(
            RoleName: "BasicUser",
            RoleDescription: LocalizationKeys.RoleSeed.BasicUserDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
            ],
            DemoUser: new SampleUser("basic.user", "basic.user@energy.local", "Ahmet", "Şahin", "Basic123!")),
    ];

    private readonly AppDbContext _db;
    private readonly IPermissionService _permissions;
    private readonly IPermissionResolver _permissionResolver;
    private readonly ApiEndpointSyncService _endpointSync;
    private readonly ILocalizationService _localization;
    private readonly PasswordHashingService _passwords;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SystemSeeder> _logger;

    public SystemSeeder(
        AppDbContext db,
        IPermissionService permissions,
        IPermissionResolver permissionResolver,
        ApiEndpointSyncService endpointSync,
        ILocalizationService localization,
        PasswordHashingService passwords,
        IConfiguration configuration,
        ILogger<SystemSeeder> logger)
    {
        _db = db;
        _permissions = permissions;
        _permissionResolver = permissionResolver;
        _endpointSync = endpointSync;
        _localization = localization;
        _passwords = passwords;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (_db.Database.IsSqlServer())
        {
            // SQL Server deployments have no migration history; build the full
            // schema from the current model in one shot. No-op if it already exists.
            _logger.LogInformation("Seeding: ensuring SQL Server schema (EnsureCreated)");
            await _db.Database.EnsureCreatedAsync(ct);
        }
        else
        {
            // PostgreSQL: migration-free top-ups that add columns/tables introduced
            // after the initial schema to pre-existing databases. Each is idempotent.
            _logger.LogInformation("Seeding: audit log schema (request/response columns)");
            await EnsureAuditSchemaAsync(ct);

            _logger.LogInformation("Seeding: direct user-permission table");
            await EnsureUserPermissionSchemaAsync(ct);

            _logger.LogInformation("Seeding: profile-image columns");
            await EnsureProfileImageSchemaAsync(ct);

            _logger.LogInformation("Seeding: chat message table");
            await EnsureChatSchemaAsync(ct);
        }

        _logger.LogInformation("Seeding: permission catalog");
        var permissionsAdded = await _permissions.SyncCatalogAsync(ct);
        _logger.LogInformation("Seeding: {Added} permission(s) added to catalog", permissionsAdded);

        _logger.LogInformation("Seeding: SuperAdmin role + admin user");
        await EnsureSuperAdminAsync(ct);

        _logger.LogInformation("Seeding: non-interactive system service account");
        await EnsureSystemServiceAccountAsync(ct);

        _logger.LogInformation("Seeding: baseline menu tree");
        await EnsureBaselineMenusAsync(ct);

        _logger.LogInformation("Seeding: API endpoint discovery + default permission mapping");
        await _endpointSync.SyncAsync(ct);

        _logger.LogInformation("Seeding: sample role templates + demo users");
        await EnsureSampleRolesAndUsersAsync(ct);

        _logger.LogInformation("Seeding: default permission grants for every role");
        await EnsureDefaultPermissionsForAllRolesAsync(ct);

        _logger.LogInformation("Seeding: localization resources (resx → DB)");
        var localizationResult = await _localization.ImportFromResxAsync(ct);
        _logger.LogInformation(
            "Localization: {Added} added, {Updated} updated, {Total} total entries.",
            localizationResult.Added, localizationResult.Updated, localizationResult.Total);
    }

    /// <summary>
    /// Idempotently adds the request/response audit columns. The project has no
    /// migration history, so this guarantees existing databases gain the new
    /// columns before any audit insert runs. Safe and no-op on fresh databases.
    /// </summary>
    private async Task EnsureAuditSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "QueryString" character varying(2000);
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "Source" character varying(10);
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "RequestBody" text;
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "ResponseBody" text;
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure AuditLogs request/response columns; they may already exist or the table is not yet created.");
        }
    }

    /// <summary>
    /// Idempotently creates the <c>UserPermissions</c> table that backs direct,
    /// per-user permission grants (managed from the User Access screen). Mirrors
    /// the migration-free approach used for the audit columns.
    /// </summary>
    private async Task EnsureUserPermissionSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS "UserPermissions" (
                "UserId" uuid NOT NULL,
                "PermissionCode" character varying(150) NOT NULL,
                CONSTRAINT "PK_UserPermissions" PRIMARY KEY ("UserId", "PermissionCode"),
                CONSTRAINT "FK_UserPermissions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_UserPermissions_Permissions_PermissionCode" FOREIGN KEY ("PermissionCode") REFERENCES "Permissions" ("Code") ON DELETE RESTRICT
            );
            CREATE INDEX IF NOT EXISTS "IX_UserPermissions_PermissionCode" ON "UserPermissions" ("PermissionCode");
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the UserPermissions table; it may already exist or a referenced table is not yet created.");
        }
    }

    /// <summary>
    /// Idempotently adds the binary profile-image columns to <c>Users</c>.
    /// Safe and no-op when they already exist (migration-free convention).
    /// </summary>
    private async Task EnsureProfileImageSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ProfileImage" bytea;
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ProfileImageContentType" character varying(100);
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the Users profile-image columns; they may already exist.");
        }
    }

    /// <summary>
    /// Idempotently creates the <c>ChatMessages</c> table backing the direct
    /// messaging feature. Mirrors the migration-free DDL approach.
    /// </summary>
    private async Task EnsureChatSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS "ChatMessages" (
                "Id" uuid NOT NULL,
                "SenderId" uuid NOT NULL,
                "RecipientId" uuid NOT NULL,
                "Text" character varying(4000) NOT NULL,
                "IsRead" boolean NOT NULL DEFAULT FALSE,
                "ReadAt" timestamp with time zone,
                "CreatedAt" timestamp with time zone NOT NULL,
                "CreatedBy" uuid,
                "UpdatedAt" timestamp with time zone,
                "UpdatedBy" uuid,
                "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                "DeletedAt" timestamp with time zone,
                "DeletedBy" uuid,
                CONSTRAINT "PK_ChatMessages" PRIMARY KEY ("Id"),
                CONSTRAINT "FK_ChatMessages_Users_SenderId" FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_ChatMessages_Users_RecipientId" FOREIGN KEY ("RecipientId") REFERENCES "Users" ("Id") ON DELETE CASCADE
            );
            CREATE INDEX IF NOT EXISTS "IX_ChatMessages_SenderId_RecipientId" ON "ChatMessages" ("SenderId", "RecipientId");
            CREATE INDEX IF NOT EXISTS "IX_ChatMessages_RecipientId_IsRead" ON "ChatMessages" ("RecipientId", "IsRead");
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the ChatMessages table; it may already exist or a referenced table is not yet created.");
        }
    }

    private async Task EnsureSuperAdminAsync(CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == SystemRoles.SuperAdmin, ct);
        if (role is null)
        {
            role = new Role
            {
                Id = Guid.NewGuid(),
                Name = SystemRoles.SuperAdmin,
                Description = LocalizationKeys.RoleSeed.SuperAdminDescription,
                IsSystem = true
            };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(ct);
        }

        const string adminEmail = "admin@energy.local";
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == adminEmail, ct);
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                PasswordHash = _passwords.Hash("Admin123!"),
                IsActive = true,
                SecurityStamp = Guid.NewGuid()
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _db.SaveChangesAsync(ct);
        }
    }

    /// <summary>
    /// Ensures a non-interactive system/service account exists and is assigned the
    /// SuperAdmin role (so it bypasses every permission check). Internal/system
    /// processes (e.g. the Web tier auditing anonymous requests) authenticate as
    /// this account to reach API endpoints independently of any signed-in user.
    /// The password is taken from configuration ("ServiceAccount:Password") and
    /// always re-asserted so the API and Web tiers stay in sync after a rotation.
    /// </summary>
    private async Task EnsureSystemServiceAccountAsync(CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == SystemRoles.SuperAdmin, ct);
        if (role is null)
        {
            // EnsureSuperAdminAsync runs first, so this should never happen.
            _logger.LogWarning("Service account seeding skipped: SuperAdmin role is missing.");
            return;
        }

        var password = _configuration[ServiceAccount.ApiPasswordConfigKey];
        if (string.IsNullOrWhiteSpace(password)) password = ServiceAccount.DefaultPassword;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == ServiceAccount.Email, ct);
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                UserName = ServiceAccount.UserName,
                Email = ServiceAccount.Email,
                FirstName = ServiceAccount.FirstName,
                LastName = ServiceAccount.LastName,
                PasswordHash = _passwords.Hash(password),
                IsActive = true,
                SecurityStamp = Guid.NewGuid()
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Service account '{UserName}' created.", ServiceAccount.UserName);
        }
        else
        {
            // Re-assert the configured password / active state so the Web tier can
            // always log in even after the secret is rotated or the account locked.
            var changed = false;
            if (!_passwords.Verify(password, user.PasswordHash))
            {
                user.PasswordHash = _passwords.Hash(password);
                user.SecurityStamp = Guid.NewGuid();
                changed = true;
            }
            if (!user.IsActive) { user.IsActive = true; changed = true; }
            if (user.LockoutEnd is not null) { user.LockoutEnd = null; changed = true; }
            if (user.FailedLoginCount != 0) { user.FailedLoginCount = 0; changed = true; }
            if (changed)
            {
                await _db.SaveChangesAsync(ct);
                _permissionResolver.InvalidateUser(user.Id);
                _logger.LogInformation("Service account '{UserName}' credentials re-asserted.", ServiceAccount.UserName);
            }
        }

        if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _db.SaveChangesAsync(ct);
        }
    }

    private async Task EnsureBaselineMenusAsync(CancellationToken ct)
    {
        // Idempotent per-node upsert keyed by NameKey. Newly introduced screens
        // (e.g. Profile) are added on a later run without wiping admin edits or
        // re-ordering an existing tree.
        var system = await EnsureMenuAsync(LocalizationKeys.Menus.System, null, null, "preferences", 10, null, ct);

        // Per-user pages every authenticated user reaches (permissions are part
        // of the DefaultGrants set so the menu is always visible).
        await EnsureMenuAsync(LocalizationKeys.Menus.Dashboard, null, "/dashboard", "home", 1, PermissionCatalog.DashboardRead, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Profile, null, "/profile", "user", 2, PermissionCatalog.ProfileRead, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Chat, null, "/chat", "message", 3, PermissionCatalog.ChatUse, ct);

        // System administration submenu — mirrors the reference project's
        // hierarchy (one entry per admin screen), each gated by the same
        // permission code the page/endpoint require.
        await EnsureMenuAsync(LocalizationKeys.Menus.Users, system.Id, "/users", "group", 11, PermissionCatalog.UserReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.UserAccess, system.Id, "/user-access", "card", 12, PermissionCatalog.UserUpdate, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Roles, system.Id, "/roles", "accountbox", 13, PermissionCatalog.RoleReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Permissions, system.Id, "/permissions", "key", 14, PermissionCatalog.PermissionReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Menus_, system.Id, "/menus", "menu", 15, PermissionCatalog.MenuReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.ApiEndpoints, system.Id, "/api-endpoints", "globe", 16, PermissionCatalog.ApiAccessReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Localization, system.Id, "/localization", "globe", 17, PermissionCatalog.LocalizationReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Logs, system.Id, "/logs", "clock", 18, PermissionCatalog.LogReadAll, ct);
    }

    private async Task<Menu> EnsureMenuAsync(
        string nameKey, Guid? parentId, string? url, string? icon, int order, string? requiredPermission, CancellationToken ct)
    {
        var menu = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == nameKey, ct);
        if (menu is not null)
        {
            // Converge the baseline structure (hierarchy, link, icon, order and
            // permission) to the current definition so existing databases adopt
            // the up-to-date menu tree without losing the node's identity/key.
            var changed =
                menu.ParentId != parentId ||
                menu.Url != url ||
                menu.Icon != icon ||
                menu.DisplayOrder != order ||
                menu.RequiredPermissionCode != requiredPermission ||
                !menu.IsActive ||
                !menu.IsVisible;

            if (changed)
            {
                menu.ParentId = parentId;
                menu.Url = url;
                menu.Icon = icon;
                menu.DisplayOrder = order;
                menu.RequiredPermissionCode = requiredPermission;
                menu.IsActive = true;
                menu.IsVisible = true;
                await _db.SaveChangesAsync(ct);
            }
            return menu;
        }

        menu = new Menu
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            NameKey = nameKey,
            Url = url,
            Icon = icon,
            DisplayOrder = order,
            RequiredPermissionCode = requiredPermission
        };
        _db.Menus.Add(menu);
        await _db.SaveChangesAsync(ct);
        return menu;
    }

    /// <summary>
    /// Grants the <see cref="PermissionCatalog.DefaultGrants"/> floor (dashboard
    /// + self-service profile) to every role except SuperAdmin (which bypasses
    /// permission checks). Guarantees that any user holding any role can always
    /// reach the dashboard and their own profile without explicit assignment.
    /// </summary>
    private async Task EnsureDefaultPermissionsForAllRolesAsync(CancellationToken ct)
    {
        var roles = await _db.Roles
            .Where(r => r.Name != SystemRoles.SuperAdmin)
            .ToListAsync(ct);

        var added = 0;
        foreach (var role in roles)
        {
            var existing = (await _db.RolePermissions
                    .Where(rp => rp.RoleId == role.Id)
                    .Select(rp => rp.PermissionCode)
                    .ToListAsync(ct))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var changed = false;
            foreach (var code in PermissionCatalog.DefaultGrants)
            {
                if (existing.Contains(code)) continue;
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionCode = code });
                added += 1;
                changed = true;
            }

            if (changed)
            {
                await _db.SaveChangesAsync(ct);
                await _permissionResolver.InvalidateRoleAsync(role.Id, ct);
            }
        }

        _logger.LogInformation("Default grants: {Added} default permission link(s) ensured across {Roles} role(s).", added, roles.Count);
    }

    private async Task EnsureSampleRolesAndUsersAsync(CancellationToken ct)
    {
        var rolesAdded = 0;
        var usersAdded = 0;
        var permissionLinks = 0;

        foreach (var sample in SampleRoles)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == sample.RoleName, ct);
            if (role is null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = sample.RoleName,
                    Description = sample.RoleDescription,
                    IsSystem = false
                };
                _db.Roles.Add(role);
                await _db.SaveChangesAsync(ct);
                rolesAdded += 1;
            }

            // Sync the permission set additively — never remove permissions an admin may have added.
            var existing = await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionCode)
                .ToListAsync(ct);
            var existingSet = existing.ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var code in sample.PermissionCodes.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (existingSet.Contains(code)) continue;
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionCode = code });
                permissionLinks += 1;
            }
            if (permissionLinks > 0) await _db.SaveChangesAsync(ct);

            // Provision the demo user once and bind it to the role.
            if (sample.DemoUser is { } demo)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == demo.Email, ct);
                if (user is null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = demo.UserName,
                        Email = demo.Email,
                        FirstName = demo.FirstName,
                        LastName = demo.LastName,
                        PasswordHash = _passwords.Hash(demo.Password),
                        IsActive = true,
                        SecurityStamp = Guid.NewGuid()
                    };
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync(ct);
                    usersAdded += 1;
                }

                if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
                {
                    _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                    await _db.SaveChangesAsync(ct);
                    _permissionResolver.InvalidateUser(user.Id);
                }
            }
        }

        _logger.LogInformation(
            "Sample catalog: {Roles} role(s) added, {Users} demo user(s) added, {Links} permission link(s) added.",
            rolesAdded, usersAdded, permissionLinks);
    }
}
