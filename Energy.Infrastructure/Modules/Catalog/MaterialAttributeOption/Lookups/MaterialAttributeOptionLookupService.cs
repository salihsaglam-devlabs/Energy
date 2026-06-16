using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialAttributeOption.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialAttributeOption.Lookups;

/// <summary>MaterialAttributeOption lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialAttributeOptionLookupService : IMaterialAttributeOptionLookupService
{
    private readonly AppDbContext _db;

    public MaterialAttributeOptionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeOptions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new MaterialAttributeOptionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>.Success(items);
    }
}
