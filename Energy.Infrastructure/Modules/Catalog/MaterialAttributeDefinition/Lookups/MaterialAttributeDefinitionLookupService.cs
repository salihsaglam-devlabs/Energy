using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Lookups;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialAttributeDefinition.Lookups;

/// <summary>MaterialAttributeDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class MaterialAttributeDefinitionLookupService : IMaterialAttributeDefinitionLookupService
{
    private readonly EnergyDbContext _db;

    public MaterialAttributeDefinitionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeDefinitions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new MaterialAttributeDefinitionLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>.Success(items);
    }
}
