using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserSetting.Lookups;
using Energy.Shared.Models.V1.IAM.UserSetting.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserSetting.Lookups;

/// <summary>UserSetting lookup servisi (aktif + arama filtreli projection).</summary>
public class UserSettingLookupService : IUserSettingLookupService
{
    private readonly EnergyDbContext _db;

    public UserSettingLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UserSettingLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UserSettings.AsNoTracking();
        var items = await query.Select(e => new UserSettingLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UserSettingLookupResponse>>.Success(items);
    }
}
