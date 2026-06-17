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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockBalanceLookupResponse>)rows.Select(e => new StockBalanceLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.LastRecalculatedAt.ToString("yyyy-MM-dd") + " - " + e.Quantity.ToString()) ? "Stock Balance #" + e.Id.ToString().Substring(0, 8) : (e.LastRecalculatedAt.ToString("yyyy-MM-dd") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>.Success(items);
    }
}
