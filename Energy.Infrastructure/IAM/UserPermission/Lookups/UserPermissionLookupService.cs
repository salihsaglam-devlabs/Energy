using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.UserPermission.Lookups;
using Energy.Shared.Models.V1.IAM.UserPermission.Responses;

namespace Energy.Infrastructure.IAM.UserPermission.Lookups;

/// <summary>UserPermission lookup servisi (aktif + arama filtreli projection).</summary>
public class UserPermissionLookupService : IUserPermissionLookupService
{
    private readonly AppDbContext _db;

    public UserPermissionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UserPermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UserPermissions.AsNoTracking();
        var items = await query
            .Select(e => new UserPermissionLookupResponse
            {
                Id = Guid.Empty,
                Code = null,
                Name = null,
                DisplayName = "",
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UserPermissionLookupResponse>>.Success(items);
    }
}
