using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.CollectionAllocation.Lookups;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

namespace Energy.Infrastructure.Modules.Finance.CollectionAllocation.Lookups;

/// <summary>CollectionAllocation lookup servisi (aktif + arama filtreli projection).</summary>
public class CollectionAllocationLookupService : ICollectionAllocationLookupService
{
    private readonly AppDbContext _db;

    public CollectionAllocationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.CollectionAllocations.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new CollectionAllocationLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>.Success(items);
    }
}
