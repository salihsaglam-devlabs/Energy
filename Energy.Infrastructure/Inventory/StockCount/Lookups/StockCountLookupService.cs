using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockCount.Lookups;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;

namespace Energy.Infrastructure.Inventory.StockCount.Lookups;

/// <summary>StockCount lookup servisi (aktif + arama filtreli projection).</summary>
public class StockCountLookupService : IStockCountLookupService
{
    private readonly AppDbContext _db;

    public StockCountLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockCountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockCounts.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.CountNo.Contains(search));
        var items = await query
            .OrderBy(e => e.CountNo)
            .Select(e => new StockCountLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.CountNo,
                DisplayName = e.CountNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockCountLookupResponse>>.Success(items);
    }
}
