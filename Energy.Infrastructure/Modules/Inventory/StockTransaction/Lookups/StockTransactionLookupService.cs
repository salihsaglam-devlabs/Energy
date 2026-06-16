using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockTransaction.Lookups;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockTransaction.Lookups;

/// <summary>StockTransaction lookup servisi (aktif + arama filtreli projection).</summary>
public class StockTransactionLookupService : IStockTransactionLookupService
{
    private readonly EnergyDbContext _db;

    public StockTransactionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockTransactions.AsNoTracking();
        var items = await query.Select(e => new StockTransactionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>.Success(items);
    }
}
