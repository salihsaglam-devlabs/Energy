using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockTransaction.Lookups;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

namespace Energy.Infrastructure.Inventory.StockTransaction.Lookups;

/// <summary>StockTransaction lookup servisi (aktif + arama filtreli projection).</summary>
public class StockTransactionLookupService : IStockTransactionLookupService
{
    private readonly AppDbContext _db;

    public StockTransactionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockTransactions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockTransactionLookupResponse>)rows.Select(e => new StockTransactionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.TransactionDate.ToString("yyyy-MM-dd") + " - " + e.Quantity.ToString()) ? "Stock Transaction #" + e.Id.ToString().Substring(0, 8) : (e.TransactionDate.ToString("yyyy-MM-dd") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>.Success(items);
    }
}
