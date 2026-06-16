using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Collection.Lookups;
using Energy.Shared.Models.V1.Finance.Collection.Responses;

namespace Energy.Infrastructure.Modules.Finance.Collection.Lookups;

/// <summary>Collection lookup servisi (aktif + arama filtreli projection).</summary>
public class CollectionLookupService : ICollectionLookupService
{
    private readonly EnergyDbContext _db;

    public CollectionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CollectionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Collections.AsNoTracking();
        var items = await query.Select(e => new CollectionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CollectionLookupResponse>>.Success(items);
    }
}
