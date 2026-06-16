using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderById;

/// <summary>Kimliğe göre PurchaseOrder detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPurchaseOrderByIdQuery(Guid Id)
    : IRequest<BaseResponse<PurchaseOrderDetailResponse>>;
