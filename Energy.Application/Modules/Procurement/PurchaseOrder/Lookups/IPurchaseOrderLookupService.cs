using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Lookups;

/// <summary>PurchaseOrder lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPurchaseOrderLookupService
{
    /// <summary>PurchaseOrder lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
