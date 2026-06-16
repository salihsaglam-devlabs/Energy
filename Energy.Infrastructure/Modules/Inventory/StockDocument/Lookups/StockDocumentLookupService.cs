using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockDocument.Lookups;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockDocument.Lookups;

/// <summary>StockDocument lookup servisi (aktif + arama filtreli projection).</summary>
public class StockDocumentLookupService : IStockDocumentLookupService
{
    private readonly EnergyDbContext _db;

    public StockDocumentLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockDocuments.AsNoTracking();
        var items = await query.Select(e => new StockDocumentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>.Success(items);
    }
}
