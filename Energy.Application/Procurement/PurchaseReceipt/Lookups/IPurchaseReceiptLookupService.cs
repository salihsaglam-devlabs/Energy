using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;

namespace Energy.Application.Procurement.PurchaseReceipt.Lookups;

/// <summary>PurchaseReceipt lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPurchaseReceiptLookupService
{
    /// <summary>PurchaseReceipt lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
