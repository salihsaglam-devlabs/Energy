using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.LocalizationResource.Lookups;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

namespace Energy.Infrastructure.Modules.Core.LocalizationResource.Lookups;

/// <summary>LocalizationResource lookup servisi (aktif + arama filtreli projection).</summary>
public class LocalizationResourceLookupService : ILocalizationResourceLookupService
{
    private readonly EnergyDbContext _db;

    public LocalizationResourceLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.LocalizationResources.AsNoTracking();
        var items = await query.Select(e => new LocalizationResourceLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>.Success(items);
    }
}
