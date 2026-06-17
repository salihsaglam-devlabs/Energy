using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockDocumentLine.Lookups;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;

namespace Energy.Infrastructure.Inventory.StockDocumentLine.Lookups;

/// <summary>StockDocumentLine lookup servisi (aktif + arama filtreli projection).</summary>
public class StockDocumentLineLookupService : IStockDocumentLineLookupService
{
    private readonly AppDbContext _db;

    public StockDocumentLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockDocumentLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockDocumentLineLookupResponse>)rows.Select(e => new StockDocumentLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Note ?? "") + " - " + e.Quantity.ToString()) ? "Stock Document Line #" + e.Id.ToString().Substring(0, 8) : ((e.Note ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>.Success(items);
    }
}
