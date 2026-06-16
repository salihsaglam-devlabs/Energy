using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Commands.DeletePurchaseReceiptLine;

/// <summary>PurchaseReceiptLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeletePurchaseReceiptLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
