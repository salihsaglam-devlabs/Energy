using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialUnitConversion.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;

namespace Energy.Infrastructure.Catalog.MaterialUnitConversion.Lookups;

/// <summary>MaterialUnitConversion lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialUnitConversionLookupService : IMaterialUnitConversionLookupService
{
    private readonly AppDbContext _db;

    public MaterialUnitConversionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialUnitConversions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<MaterialUnitConversionLookupResponse>)rows.Select(e => new MaterialUnitConversionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Factor.ToString()) ? "Material Unit Conversion #" + e.Id.ToString().Substring(0, 8) : (e.Factor.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>.Success(items);
    }
}
