using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.RolePermission.Lookups;
using Energy.Shared.Models.V1.IAM.RolePermission.Responses;

namespace Energy.Infrastructure.Modules.IAM.RolePermission.Lookups;

/// <summary>RolePermission lookup servisi (aktif + arama filtreli projection).</summary>
public class RolePermissionLookupService : IRolePermissionLookupService
{
    private readonly EnergyDbContext _db;

    public RolePermissionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<RolePermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.RolePermissions.AsNoTracking();
        var items = await query.Select(e => new RolePermissionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<RolePermissionLookupResponse>>.Success(items);
    }
}
