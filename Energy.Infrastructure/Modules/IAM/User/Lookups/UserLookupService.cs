using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.User.Lookups;
using Energy.Shared.Models.V1.IAM.User.Responses;

namespace Energy.Infrastructure.Modules.IAM.User.Lookups;

/// <summary>User lookup servisi (aktif + arama filtreli projection).</summary>
public class UserLookupService : IUserLookupService
{
    private readonly EnergyDbContext _db;

    public UserLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UserLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Users.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var items = await query.Select(e => new UserLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UserLookupResponse>>.Success(items);
    }
}
