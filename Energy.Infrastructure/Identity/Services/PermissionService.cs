using Energy.Application.Identity.Services;
using Energy.Domain.Modules.IAM;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Identity.Services;

public sealed class PermissionService : IPermissionService
{
    private readonly AppDbContext _db;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PermissionService(AppDbContext db, IStringLocalizer<SharedResource> localizer)
    {
        _db = db;
        _localizer = localizer;
    }

    public async Task<IReadOnlyList<PermissionResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var rows = await _db.Permissions.AsNoTracking().ToListAsync(ct);
        var roleCounts = await _db.RolePermissions.AsNoTracking()
            .GroupBy(rp => rp.PermissionCode)
            .Select(g => new { Code = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Code, x => x.Count, ct);

        var menuCounts = await _db.Menus.AsNoTracking()
            .Where(m => m.RequiredPermissionCode != null)
            .GroupBy(m => m.RequiredPermissionCode!)
            .Select(g => new { Code = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Code, x => x.Count, ct);

        var endpointCounts = await _db.ApiEndpoints.AsNoTracking()
            .Where(e => e.RequiredPermissionCode != null)
            .GroupBy(e => e.RequiredPermissionCode!)
            .Select(g => new { Code = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Code, x => x.Count, ct);

        return rows.OrderBy(r => r.Module).ThenBy(r => r.Action)
            .Select(r => Map(r, roleCounts, menuCounts, endpointCounts))
            .ToList();
    }

    public async Task<PermissionResponse?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var row = await _db.Permissions.AsNoTracking().FirstOrDefaultAsync(p => p.Code == code, ct);
        if (row is null) return null;
        var roleCount = await _db.RolePermissions.AsNoTracking().CountAsync(rp => rp.PermissionCode == code, ct);
        var menuCount = await _db.Menus.AsNoTracking().CountAsync(m => m.RequiredPermissionCode == code, ct);
        var endpointCount = await _db.ApiEndpoints.AsNoTracking().CountAsync(e => e.RequiredPermissionCode == code, ct);
        return Map(row,
            new Dictionary<string, int> { [code] = roleCount },
            new Dictionary<string, int> { [code] = menuCount },
            new Dictionary<string, int> { [code] = endpointCount });
    }

    public async Task<int> SyncCatalogAsync(CancellationToken ct = default)
    {
        var existing = await _db.Permissions.ToDictionaryAsync(p => p.Code, ct);
        var added = 0;

        foreach (var descriptor in PermissionCatalog.All)
        {
            if (existing.TryGetValue(descriptor.Code, out var row))
            {
                row.Module = descriptor.Module;
                row.Action = descriptor.Action;
                row.DisplayNameKey = descriptor.DisplayNameKey;
                row.DescriptionKey = descriptor.DescriptionKey;
            }
            else
            {
                _db.Permissions.Add(new Permission
                {
                    Code = descriptor.Code,
                    Module = descriptor.Module,
                    Action = descriptor.Action,
                    DisplayNameKey = descriptor.DisplayNameKey,
                    DescriptionKey = descriptor.DescriptionKey
                });
                added += 1;
            }
        }

        await _db.SaveChangesAsync(ct);
        return added;
    }

    private PermissionResponse Map(Permission p,
        IReadOnlyDictionary<string, int> roles,
        IReadOnlyDictionary<string, int> menus,
        IReadOnlyDictionary<string, int> endpoints)
    {
        var displayName = _localizer[p.DisplayNameKey].Value;
        if (string.IsNullOrEmpty(displayName) || displayName == p.DisplayNameKey)
        {
            displayName = $"{p.Module} {p.Action}";
        }
        var description = p.DescriptionKey is null ? null : _localizer[p.DescriptionKey].Value;
        if (description == p.DescriptionKey) description = null;

        return new PermissionResponse
        {
            Code = p.Code,
            Module = p.Module,
            Action = p.Action,
            DisplayName = displayName,
            Description = description,
            RoleCount = roles.GetValueOrDefault(p.Code),
            MenuCount = menus.GetValueOrDefault(p.Code),
            EndpointCount = endpoints.GetValueOrDefault(p.Code)
        };
    }
}
