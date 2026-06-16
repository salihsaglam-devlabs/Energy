using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.PurchaseReceipt.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;

namespace Energy.Infrastructure.Modules.Procurement.PurchaseReceipt.Lookups;

/// <summary>PurchaseReceipt lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseReceiptLookupService : IPurchaseReceiptLookupService
{
    private readonly EnergyDbContext _db;

    public PurchaseReceiptLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseReceipts.AsNoTracking();
        var items = await query.Select(e => new PurchaseReceiptLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>.Success(items);
    }
}
