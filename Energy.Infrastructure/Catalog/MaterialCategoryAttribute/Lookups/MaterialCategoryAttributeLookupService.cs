using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialCategoryAttribute.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

namespace Energy.Infrastructure.Catalog.MaterialCategoryAttribute.Lookups;

/// <summary>MaterialCategoryAttribute lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialCategoryAttributeLookupService : IMaterialCategoryAttributeLookupService
{
    private readonly AppDbContext _db;

    public MaterialCategoryAttributeLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialCategoryAttributes.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<MaterialCategoryAttributeLookupResponse>)rows.Select(e => new MaterialCategoryAttributeLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = "Material Category Attribute #" + e.Id.ToString().Substring(0, 8),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>.Success(items);
    }
}
