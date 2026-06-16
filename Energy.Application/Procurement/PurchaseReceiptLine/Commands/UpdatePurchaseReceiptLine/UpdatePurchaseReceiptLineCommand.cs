using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Commands.UpdatePurchaseReceiptLine;

/// <summary>Var olan PurchaseReceiptLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePurchaseReceiptLineCommand(Guid Id, UpdatePurchaseReceiptLineRequest Request)
    : IRequest<BaseResponse<bool>>;
