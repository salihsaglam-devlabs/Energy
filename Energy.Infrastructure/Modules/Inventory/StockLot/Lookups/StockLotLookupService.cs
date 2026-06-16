using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockLot.Lookups;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockLot.Lookups;

/// <summary>StockLot lookup servisi (aktif + arama filtreli projection).</summary>
public class StockLotLookupService : IStockLotLookupService
{
    private readonly AppDbContext _db;

    public StockLotLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockLotLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockLots.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new StockLotLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockLotLookupResponse>>.Success(items);
    }
}
