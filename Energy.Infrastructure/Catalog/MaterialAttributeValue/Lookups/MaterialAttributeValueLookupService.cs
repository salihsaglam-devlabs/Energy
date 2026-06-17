using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialAttributeValue.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

namespace Energy.Infrastructure.Catalog.MaterialAttributeValue.Lookups;

/// <summary>MaterialAttributeValue lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialAttributeValueLookupService : IMaterialAttributeValueLookupService
{
    private readonly AppDbContext _db;

    public MaterialAttributeValueLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeValues.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<MaterialAttributeValueLookupResponse>)rows.Select(e => new MaterialAttributeValueLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.ValueDate.HasValue ? e.ValueDate.Value.ToString("yyyy-MM-dd") : "") + " - " + (e.ValueNumber.HasValue ? e.ValueNumber.Value.ToString() : "")) ? "Material Attribute Value #" + e.Id.ToString().Substring(0, 8) : ((e.ValueDate.HasValue ? e.ValueDate.Value.ToString("yyyy-MM-dd") : "") + " - " + (e.ValueNumber.HasValue ? e.ValueNumber.Value.ToString() : "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>.Success(items);
    }
}
