using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.UnitConversion.Lookups;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;

namespace Energy.Infrastructure.Core.UnitConversion.Lookups;

/// <summary>UnitConversion lookup servisi (aktif + arama filtreli projection).</summary>
public class UnitConversionLookupService : IUnitConversionLookupService
{
    private readonly AppDbContext _db;

    public UnitConversionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UnitConversions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<UnitConversionLookupResponse>)rows.Select(e => new UnitConversionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Factor.ToString()) ? "Unit Conversion #" + e.Id.ToString().Substring(0, 8) : (e.Factor.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>.Success(items);
    }
}
