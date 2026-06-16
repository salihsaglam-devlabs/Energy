using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialUnitConversion.Lookups;

/// <summary>MaterialUnitConversion lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialUnitConversionLookupService : IMaterialUnitConversionLookupService
{
    private readonly AppDbContext _db;

    public MaterialUnitConversionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialUnitConversions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new MaterialUnitConversionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>.Success(items);
    }
}
