using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Commands.DeletePurchaseOrder;

/// <summary>PurchaseOrder kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeletePurchaseOrderCommand(Guid Id) : IRequest<BaseResponse<bool>>;

