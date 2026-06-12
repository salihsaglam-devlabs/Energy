using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Identity.Permissions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.System.Services;

/// <summary>
/// Scans <see cref="ApiDescription"/> at startup and registers every (method, path)
/// the application exposes. Rows that the seeder recognises from
/// <see cref="DefaultEndpointPermissionMap"/> are inserted as ACTIVE with the
/// matching permission so the system is usable out-of-the-box. Unknown routes
/// remain inactive with no permission — default DENY is preserved until an
/// admin reviews them. Rows that already exist are never overwritten so manual
/// edits in the UI survive every restart.
/// </summary>
public sealed class ApiEndpointSyncService
{
    /// <summary>
    /// Convention map keyed by <c>"Controller.Action"</c> (case-insensitive).
    /// Value is the required permission code; <c>null</c> marks the route as
    /// public (active, no permission needed — e.g. login, "my menu").
    /// </summary>
    private static readonly IReadOnlyDictionary<string, string?> DefaultEndpointPermissionMap =
        new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            // Auth — login is anonymous; the row exists for visibility in the admin UI.
            ["Auth.Login"] = null,

            // Home / Dashboard
            ["Home.GetDashboard"] = PermissionCatalog.DashboardRead,

            // Users
            ["Users.GetAll"]         = PermissionCatalog.UserReadAll,
            ["Users.GetById"]        = PermissionCatalog.UserRead,
            ["Users.Create"]         = PermissionCatalog.UserCreate,
            ["Users.Update"]         = PermissionCatalog.UserUpdate,
            ["Users.Delete"]         = PermissionCatalog.UserDelete,
            ["Users.ChangePassword"] = PermissionCatalog.UserUpdate,
            ["Users.GetProfileImage"] = PermissionCatalog.ProfileRead,
            ["Users.SetProfileImage"] = PermissionCatalog.ProfileUpdate,
            ["Users.RemoveProfileImage"] = PermissionCatalog.ProfileUpdate,
            ["Users.GetAccess"]      = PermissionCatalog.UserReadAll,
            ["Users.SetAccess"]      = PermissionCatalog.UserUpdate,

            // Roles
            ["Roles.GetAll"]         = PermissionCatalog.RoleReadAll,
            ["Roles.GetById"]        = PermissionCatalog.RoleRead,
            ["Roles.Create"]         = PermissionCatalog.RoleCreate,
            ["Roles.Update"]         = PermissionCatalog.RoleUpdate,
            ["Roles.Delete"]         = PermissionCatalog.RoleDelete,
            ["Roles.SetPermissions"] = PermissionCatalog.RoleUpdate,

            // Permissions (read-only from UI)
            ["Permissions.GetAll"]    = PermissionCatalog.PermissionReadAll,
            ["Permissions.GetByCode"] = PermissionCatalog.PermissionRead,

            // Menus — "me" returns the current user's tree, must work for any signed-in user.
            ["Menus.GetAll"]    = PermissionCatalog.MenuReadAll,
            ["Menus.GetById"]   = PermissionCatalog.MenuRead,
            ["Menus.GetMyMenu"] = null,
            ["Menus.Create"]    = PermissionCatalog.MenuCreate,
            ["Menus.Update"]    = PermissionCatalog.MenuUpdate,
            ["Menus.Delete"]    = PermissionCatalog.MenuDelete,

            // API endpoints
            ["ApiEndpoints.GetAll"]  = PermissionCatalog.ApiAccessReadAll,
            ["ApiEndpoints.GetById"] = PermissionCatalog.ApiAccessRead,
            ["ApiEndpoints.Create"]  = PermissionCatalog.ApiAccessCreate,
            ["ApiEndpoints.Update"]  = PermissionCatalog.ApiAccessUpdate,
            ["ApiEndpoints.Delete"]  = PermissionCatalog.ApiAccessDelete,

            // Localization
            ["Localization.GetAll"]   = PermissionCatalog.LocalizationReadAll,
            ["Localization.GetByKey"] = PermissionCatalog.LocalizationRead,
            ["Localization.Upsert"]   = PermissionCatalog.LocalizationUpdate,
            ["Localization.Delete"]   = PermissionCatalog.LocalizationDelete,

            // Seed — high-privilege maintenance operations gated by System.Seed.
            ["Seed.SeedAll"]                  = PermissionCatalog.SystemSeed,
            ["Seed.SeedLocalization"]         = PermissionCatalog.SystemSeed,
            ["Seed.SeedLocalizationFromResx"] = PermissionCatalog.SystemSeed,

            // Audit logs
            ["AuditLogs.Query"]   = PermissionCatalog.LogReadAll,
            ["AuditLogs.GetById"] = PermissionCatalog.LogRead,
            // Ingest is used by upper layers (Web) to record their own request
            // logs; any authenticated user may post their own navigation entry.
            ["AuditLogs.Ingest"]  = null,

            // Chat — every authenticated user collaborates; ships as a default grant.
            ["Chat.GetContacts"]     = PermissionCatalog.ChatUse,
            ["Chat.GetConversation"] = PermissionCatalog.ChatUse,
            ["Chat.Send"]            = PermissionCatalog.ChatUse,
            ["Chat.MarkRead"]        = PermissionCatalog.ChatUse,
            ["Chat.UnreadCount"]     = PermissionCatalog.ChatUse,
            ["Chat.GetAttachment"]   = PermissionCatalog.ChatUse,
            ["Chat.GetUserAvatar"]   = PermissionCatalog.ChatUse,
        };

    private readonly AppDbContext _db;
    private readonly IApiDescriptionGroupCollectionProvider _descriptions;
    private readonly ILogger<ApiEndpointSyncService> _logger;

    public ApiEndpointSyncService(
        AppDbContext db,
        IApiDescriptionGroupCollectionProvider descriptions,
        ILogger<ApiEndpointSyncService> logger)
    {
        _db = db;
        _descriptions = descriptions;
        _logger = logger;
    }

    public async Task SyncAsync(CancellationToken ct = default)
    {
        var discovered = _descriptions.ApiDescriptionGroups.Items
            .SelectMany(group => group.Items)
            .Where(d => !string.IsNullOrWhiteSpace(d.RelativePath))
            .Select(d => new
            {
                Method = (d.HttpMethod ?? "GET").ToUpperInvariant(),
                Path = "/" + d.RelativePath!.TrimStart('/'),
                Controller = RouteValue(d, "controller"),
                Action = RouteValue(d, "action")
            })
            .DistinctBy(x => (x.Method, x.Path))
            .ToList();

        var existing = await _db.ApiEndpoints.ToListAsync(ct);
        var existingByKey = existing.ToDictionary(
            e => (e.HttpMethod.ToUpperInvariant(), e.Path),
            e => e);

        var added = 0;
        var activated = 0;

        foreach (var d in discovered)
        {
            var convention = $"{d.Controller}.{d.Action}";
            var hasDefault = DefaultEndpointPermissionMap.TryGetValue(convention, out var defaultPermission);

            if (existingByKey.TryGetValue((d.Method, d.Path), out var row))
            {
                // Heuristic: only auto-configure rows that the previous sync inserted
                // as INACTIVE with NO permission — i.e. rows the admin has never touched.
                if (hasDefault && !row.IsActive && row.RequiredPermissionCode is null)
                {
                    row.IsActive = true;
                    row.RequiredPermissionCode = defaultPermission;
                    activated += 1;
                }
                continue;
            }

            _db.ApiEndpoints.Add(new ApiEndpoint
            {
                Id = Guid.NewGuid(),
                Name = convention,
                Path = d.Path,
                HttpMethod = d.Method,
                IsActive = hasDefault,
                RequiredPermissionCode = hasDefault ? defaultPermission : null
            });
            added += 1;
        }

        if (added > 0 || activated > 0)
        {
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation(
                "ApiEndpoint sync: {Added} new endpoint(s), {Activated} auto-activated from defaults.",
                added, activated);
        }
    }

    private static string RouteValue(ApiDescription d, string key)
        => d.ActionDescriptor.RouteValues.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value)
            ? value
            : "Unknown";
}
