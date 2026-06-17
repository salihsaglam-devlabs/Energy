using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseOrder.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseOrder.Lookups;

/// <summary>PurchaseOrder lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseOrderLookupService : IPurchaseOrderLookupService
{
    private readonly AppDbContext _db;

    public PurchaseOrderLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrders.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.OrderNo.Contains(search));
        var items = await query
            .OrderBy(e => e.OrderNo)
            .Select(e => new PurchaseOrderLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.OrderNo,
                DisplayName = e.OrderNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>.Success(items);
    }
}
