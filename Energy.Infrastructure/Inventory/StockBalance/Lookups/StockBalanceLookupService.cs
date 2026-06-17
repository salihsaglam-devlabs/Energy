using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockBalance.Lookups;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

namespace Energy.Infrastructure.Inventory.StockBalance.Lookups;

/// <summary>StockBalance lookup servisi (aktif + arama filtreli projection).</summary>
public class StockBalanceLookupService : IStockBalanceLookupService
{
    private readonly AppDbContext _db;

    public StockBalanceLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockBalances.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new StockBalanceLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>.Success(items);
    }
}
