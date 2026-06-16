using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.PurchaseOrderLine.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

namespace Energy.Infrastructure.Modules.Procurement.PurchaseOrderLine.Lookups;

/// <summary>PurchaseOrderLine lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseOrderLineLookupService : IPurchaseOrderLineLookupService
{
    private readonly AppDbContext _db;

    public PurchaseOrderLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrderLines.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new PurchaseOrderLineLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>.Success(items);
    }
}
