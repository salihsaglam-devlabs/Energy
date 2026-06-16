using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.Brand.Lookups;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;

namespace Energy.Infrastructure.Modules.Catalog.Brand.Lookups;

/// <summary>Brand lookup servisi (aktif + arama filtreli projection).</summary>
public class BrandLookupService : IBrandLookupService
{
    private readonly AppDbContext _db;

    public BrandLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BrandLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Brands.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new BrandLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BrandLookupResponse>>.Success(items);
    }
}
