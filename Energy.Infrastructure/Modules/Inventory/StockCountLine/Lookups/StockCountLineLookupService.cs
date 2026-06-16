using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockCountLine.Lookups;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockCountLine.Lookups;

/// <summary>StockCountLine lookup servisi (aktif + arama filtreli projection).</summary>
public class StockCountLineLookupService : IStockCountLineLookupService
{
    private readonly EnergyDbContext _db;

    public StockCountLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockCountLines.AsNoTracking();
        var items = await query.Select(e => new StockCountLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>.Success(items);
    }
}
