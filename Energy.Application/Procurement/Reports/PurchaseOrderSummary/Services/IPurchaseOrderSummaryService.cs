using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;

namespace Energy.Application.Procurement.Reports.PurchaseOrderSummary.Services;

/// <summary>PurchaseOrderSummary raporu servis sözleşmesi (salt-okunur).</summary>
public interface IPurchaseOrderSummaryService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>> GetDataAsync(PurchaseOrderSummaryRequest request, CancellationToken ct = default);
}
