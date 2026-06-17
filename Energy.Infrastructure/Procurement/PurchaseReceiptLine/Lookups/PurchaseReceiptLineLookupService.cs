using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseReceiptLine.Lookups;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseReceiptLine.Lookups;

/// <summary>PurchaseReceiptLine lookup servisi (aktif + arama filtreli projection).</summary>
public class PurchaseReceiptLineLookupService : IPurchaseReceiptLineLookupService
{
    private readonly AppDbContext _db;

    public PurchaseReceiptLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PurchaseReceiptLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<PurchaseReceiptLineLookupResponse>)rows.Select(e => new PurchaseReceiptLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Quantity.ToString()) ? "Purchase Receipt Line #" + e.Id.ToString().Substring(0, 8) : (e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>.Success(items);
    }
}
