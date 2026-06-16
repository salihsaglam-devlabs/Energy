using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Lookups;

/// <summary>PurchaseReceiptLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPurchaseReceiptLineLookupService
{
    /// <summary>PurchaseReceiptLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
