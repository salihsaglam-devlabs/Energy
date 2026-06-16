using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockCount.Lookups;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockCount.Lookups;

/// <summary>StockCount lookup servisi (aktif + arama filtreli projection).</summary>
public class StockCountLookupService : IStockCountLookupService
{
    private readonly EnergyDbContext _db;

    public StockCountLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockCountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockCounts.AsNoTracking();
        var items = await query.Select(e => new StockCountLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockCountLookupResponse>>.Success(items);
    }
}
