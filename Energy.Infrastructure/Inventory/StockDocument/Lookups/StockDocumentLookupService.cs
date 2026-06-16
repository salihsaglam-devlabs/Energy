using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockDocument.Lookups;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

namespace Energy.Infrastructure.Inventory.StockDocument.Lookups;

/// <summary>StockDocument lookup servisi (aktif + arama filtreli projection).</summary>
public class StockDocumentLookupService : IStockDocumentLookupService
{
    private readonly AppDbContext _db;

    public StockDocumentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockDocuments.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.DocumentNo.Contains(search));
        var items = await query
            .OrderBy(e => e.DocumentNo)
            .Select(e => new StockDocumentLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.DocumentNo,
                DisplayName = e.DocumentNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>.Success(items);
    }
}
