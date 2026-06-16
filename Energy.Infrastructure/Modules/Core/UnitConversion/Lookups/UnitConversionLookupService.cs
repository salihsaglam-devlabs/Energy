using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.UnitConversion.Lookups;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;

namespace Energy.Infrastructure.Modules.Core.UnitConversion.Lookups;

/// <summary>UnitConversion lookup servisi (aktif + arama filtreli projection).</summary>
public class UnitConversionLookupService : IUnitConversionLookupService
{
    private readonly AppDbContext _db;

    public UnitConversionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UnitConversions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new UnitConversionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>.Success(items);
    }
}
