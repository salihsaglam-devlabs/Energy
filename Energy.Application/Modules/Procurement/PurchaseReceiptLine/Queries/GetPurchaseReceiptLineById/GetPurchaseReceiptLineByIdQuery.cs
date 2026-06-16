using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineById;

/// <summary>Kimliğe göre PurchaseReceiptLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPurchaseReceiptLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<PurchaseReceiptLineDetailResponse>>;
