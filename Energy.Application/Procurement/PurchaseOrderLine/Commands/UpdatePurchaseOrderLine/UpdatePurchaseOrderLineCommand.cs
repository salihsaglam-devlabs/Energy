using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using MediatR;

namespace Energy.Application.Procurement.PurchaseOrderLine.Commands.UpdatePurchaseOrderLine;

/// <summary>Var olan PurchaseOrderLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePurchaseOrderLineCommand(Guid Id, UpdatePurchaseOrderLineRequest Request)
    : IRequest<BaseResponse<bool>>;
