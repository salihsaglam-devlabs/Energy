using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.Material.Lookups;
using Energy.Shared.Models.V1.Catalog.Material.Responses;

namespace Energy.Infrastructure.Catalog.Material.Lookups;

/// <summary>Material lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialLookupService : IMaterialLookupService
{
    private readonly AppDbContext _db;

    public MaterialLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Materials.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new MaterialLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MaterialLookupResponse>>.Success(items);
    }
}
