using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineList;

/// <summary>Sayfalanmış PurchaseOrderLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPurchaseOrderLineListQuery(GetPurchaseOrderLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>>;
