using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Commands.UpdatePurchaseOrder;

/// <summary>Var olan PurchaseOrder kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePurchaseOrderCommand(Guid Id, UpdatePurchaseOrderRequest Request)
    : IRequest<BaseResponse<bool>>;

