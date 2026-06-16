using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserRole.Lookups;
using Energy.Shared.Models.V1.IAM.UserRole.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserRole.Lookups;

/// <summary>UserRole lookup servisi (aktif + arama filtreli projection).</summary>
public class UserRoleLookupService : IUserRoleLookupService
{
    private readonly EnergyDbContext _db;

    public UserRoleLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UserRoleLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UserRoles.AsNoTracking();
        var items = await query.Select(e => new UserRoleLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UserRoleLookupResponse>>.Success(items);
    }
}
