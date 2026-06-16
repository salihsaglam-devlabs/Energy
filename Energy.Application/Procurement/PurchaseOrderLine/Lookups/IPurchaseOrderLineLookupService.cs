using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

namespace Energy.Application.Procurement.PurchaseOrderLine.Lookups;

/// <summary>PurchaseOrderLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPurchaseOrderLineLookupService
{
    /// <summary>PurchaseOrderLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
