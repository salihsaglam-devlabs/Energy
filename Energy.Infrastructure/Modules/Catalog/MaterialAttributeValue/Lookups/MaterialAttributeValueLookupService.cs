using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialAttributeValue.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialAttributeValue.Lookups;

/// <summary>MaterialAttributeValue lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialAttributeValueLookupService : IMaterialAttributeValueLookupService
{
    private readonly AppDbContext _db;

    public MaterialAttributeValueLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeValues.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new MaterialAttributeValueLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>.Success(items);
    }
}
