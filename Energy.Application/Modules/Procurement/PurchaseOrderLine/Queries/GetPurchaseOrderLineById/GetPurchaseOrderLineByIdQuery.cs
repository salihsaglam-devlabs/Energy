using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineById;

/// <summary>Kimliğe göre PurchaseOrderLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPurchaseOrderLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<PurchaseOrderLineDetailResponse>>;
