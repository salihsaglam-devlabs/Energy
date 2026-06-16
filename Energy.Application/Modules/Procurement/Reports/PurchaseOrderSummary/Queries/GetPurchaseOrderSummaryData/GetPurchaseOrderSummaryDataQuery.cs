using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.Reports.PurchaseOrderSummary.Queries.GetPurchaseOrderSummaryData;

/// <summary>PurchaseOrderSummary rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetPurchaseOrderSummaryDataQuery(PurchaseOrderSummaryRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>>;
