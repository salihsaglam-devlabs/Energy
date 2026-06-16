using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockDocumentLine.Lookups;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockDocumentLine.Lookups;

/// <summary>StockDocumentLine lookup servisi (aktif + arama filtreli projection).</summary>
public class StockDocumentLineLookupService : IStockDocumentLineLookupService
{
    private readonly EnergyDbContext _db;

    public StockDocumentLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockDocumentLines.AsNoTracking();
        var items = await query.Select(e => new StockDocumentLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>.Success(items);
    }
}
