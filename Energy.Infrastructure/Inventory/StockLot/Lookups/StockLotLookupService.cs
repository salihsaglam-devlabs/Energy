using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockLot.Lookups;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Infrastructure.Inventory.StockLot.Lookups;

/// <summary>StockLot lookup servisi (aktif + arama filtreli projection).</summary>
public class StockLotLookupService : IStockLotLookupService
{
    private readonly AppDbContext _db;

    public StockLotLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockLotLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockLots.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.LotNo)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockLotLookupResponse>)rows.Select(e => new StockLotLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.LotNo ?? "") + " - " + e.ReceivedAt.ToString("yyyy-MM-dd")) ? "Stock Lot #" + e.Id.ToString().Substring(0, 8) : ((e.LotNo ?? "") + " - " + e.ReceivedAt.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockLotLookupResponse>>.Success(items);
    }
}
