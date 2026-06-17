using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseOrderLine.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseOrderLine.Lookups;

/// <summary>PurchaseOrderLine lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseOrderLineLookupService : IPurchaseOrderLineLookupService
{
    private readonly AppDbContext _db;

    public PurchaseOrderLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrderLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<PurchaseOrderLineLookupResponse>)rows.Select(e => new PurchaseOrderLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Quantity.ToString()) ? "Purchase Order Line #" + e.Id.ToString().Substring(0, 8) : (e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>.Success(items);
    }
}
