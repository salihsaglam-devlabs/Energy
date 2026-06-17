using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseReceipt.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseReceipt.Lookups;

/// <summary>PurchaseReceipt lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseReceiptLookupService : IPurchaseReceiptLookupService
{
    private readonly AppDbContext _db;

    public PurchaseReceiptLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseReceipts.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.ReceiptNo.Contains(search));
        var items = await query
            .OrderBy(e => e.ReceiptNo)
            .Select(e => new PurchaseReceiptLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.ReceiptNo,
                DisplayName = e.ReceiptNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>.Success(items);
    }
}
