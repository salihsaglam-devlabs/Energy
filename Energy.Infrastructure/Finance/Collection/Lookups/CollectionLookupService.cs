using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.Collection.Lookups;
using Energy.Shared.Models.V1.Finance.Collection.Responses;

namespace Energy.Infrastructure.Finance.Collection.Lookups;

/// <summary>Collection lookup servisi (aktif + arama filtreli projection).</summary>
public class CollectionLookupService : ICollectionLookupService
{
    private readonly AppDbContext _db;

    public CollectionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CollectionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Collections.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.CollectionNo.Contains(search));
        var items = await query
            .OrderBy(e => e.CollectionNo)
            .Select(e => new CollectionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.CollectionNo,
                DisplayName = e.CollectionNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CollectionLookupResponse>>.Success(items);
    }
}
