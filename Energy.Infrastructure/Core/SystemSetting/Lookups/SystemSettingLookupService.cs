using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.SystemSetting.Lookups;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;

namespace Energy.Infrastructure.Core.SystemSetting.Lookups;

/// <summary>SystemSetting lookup servisi (aktif + arama filtreli projection).</summary>
public class SystemSettingLookupService : ISystemSettingLookupService
{
    private readonly AppDbContext _db;

    public SystemSettingLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SystemSettings.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Key)
            .Select(e => new SystemSettingLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Key,
                DisplayName = e.Key,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>.Success(items);
    }
}
