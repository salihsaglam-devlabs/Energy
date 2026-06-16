using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Commands.DeletePurchaseReceipt;

/// <summary>PurchaseReceipt kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeletePurchaseReceiptCommand(Guid Id) : IRequest<BaseResponse<bool>>;
