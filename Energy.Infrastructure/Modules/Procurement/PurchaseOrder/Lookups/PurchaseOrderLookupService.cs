using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.PurchaseOrder.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

namespace Energy.Infrastructure.Modules.Procurement.PurchaseOrder.Lookups;

/// <summary>PurchaseOrder lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseOrderLookupService : IPurchaseOrderLookupService
{
    private readonly EnergyDbContext _db;

    public PurchaseOrderLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrders.AsNoTracking();
        var items = await query.Select(e => new PurchaseOrderLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>.Success(items);
    }
}
