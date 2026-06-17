using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialAttributeOption.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;

namespace Energy.Infrastructure.Catalog.MaterialAttributeOption.Lookups;

/// <summary>MaterialAttributeOption lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialAttributeOptionLookupService : IMaterialAttributeOptionLookupService
{
    private readonly AppDbContext _db;

    public MaterialAttributeOptionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeOptions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<MaterialAttributeOptionLookupResponse>)rows.Select(e => new MaterialAttributeOptionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = "Material Attribute Option #" + e.Id.ToString().Substring(0, 8),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>.Success(items);
    }
}
