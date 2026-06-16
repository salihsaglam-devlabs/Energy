using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Commands.UpdatePurchaseReceipt;

/// <summary>Var olan PurchaseReceipt kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePurchaseReceiptCommand(Guid Id, UpdatePurchaseReceiptRequest Request)
    : IRequest<BaseResponse<bool>>;
