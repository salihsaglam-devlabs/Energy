using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockDocumentType.Lookups;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;

namespace Energy.Infrastructure.Inventory.StockDocumentType.Lookups;

/// <summary>StockDocumentType lookup servisi (aktif + arama filtreli projection).</summary>
public class StockDocumentTypeLookupService : IStockDocumentTypeLookupService
{
    private readonly AppDbContext _db;

    public StockDocumentTypeLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockDocumentTypes.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new StockDocumentTypeLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>.Success(items);
    }
}
