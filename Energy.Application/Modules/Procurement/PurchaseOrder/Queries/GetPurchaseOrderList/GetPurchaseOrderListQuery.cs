using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderList;

/// <summary>Sayfalanmış PurchaseOrder listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPurchaseOrderListQuery(GetPurchaseOrderListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>>;

