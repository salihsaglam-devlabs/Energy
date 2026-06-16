using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserPermission.Lookups;
using Energy.Shared.Models.V1.IAM.UserPermission.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserPermission.Lookups;

/// <summary>UserPermission lookup servisi (aktif + arama filtreli projection).</summary>
public class UserPermissionLookupService : IUserPermissionLookupService
{
    private readonly EnergyDbContext _db;

    public UserPermissionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UserPermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UserPermissions.AsNoTracking();
        var items = await query.Select(e => new UserPermissionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UserPermissionLookupResponse>>.Success(items);
    }
}
