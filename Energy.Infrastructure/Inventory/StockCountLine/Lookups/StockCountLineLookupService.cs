using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockCountLine.Lookups;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

namespace Energy.Infrastructure.Inventory.StockCountLine.Lookups;

/// <summary>StockCountLine lookup servisi (aktif + arama filtreli projection).</summary>
public class StockCountLineLookupService : IStockCountLineLookupService
{
    private readonly AppDbContext _db;

    public StockCountLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockCountLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockCountLineLookupResponse>)rows.Select(e => new StockCountLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.SystemQuantity.ToString()) ? "Stock Count Line #" + e.Id.ToString().Substring(0, 8) : (e.SystemQuantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>.Success(items);
    }
}
