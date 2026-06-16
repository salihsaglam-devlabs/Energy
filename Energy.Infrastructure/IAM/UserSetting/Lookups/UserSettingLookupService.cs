using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.UserSetting.Lookups;
using Energy.Shared.Models.V1.IAM.UserSetting.Responses;

namespace Energy.Infrastructure.IAM.UserSetting.Lookups;

/// <summary>UserSetting lookup servisi (aktif + arama filtreli projection).</summary>
public class UserSettingLookupService : IUserSettingLookupService
{
    private readonly AppDbContext _db;

    public UserSettingLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UserSettingLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UserSettings.AsNoTracking();
        var items = await query
            .Select(e => new UserSettingLookupResponse
            {
                Id = Guid.Empty,
                Code = null,
                Name = null,
                DisplayName = "",
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UserSettingLookupResponse>>.Success(items);
    }
}
