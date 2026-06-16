using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptById;

/// <summary>Kimliğe göre PurchaseReceipt detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPurchaseReceiptByIdQuery(Guid Id)
    : IRequest<BaseResponse<PurchaseReceiptDetailResponse>>;
