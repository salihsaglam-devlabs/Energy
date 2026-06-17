using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialCategory.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;

namespace Energy.Infrastructure.Catalog.MaterialCategory.Lookups;

/// <summary>MaterialCategory lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialCategoryLookupService : IMaterialCategoryLookupService
{
    private readonly AppDbContext _db;

    public MaterialCategoryLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialCategories.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new MaterialCategoryLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>.Success(items);
    }
}
