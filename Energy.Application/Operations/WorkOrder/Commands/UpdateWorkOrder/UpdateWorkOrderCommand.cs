using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Commands.UpdateWorkOrder;

/// <summary>Var olan WorkOrder kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderCommand(Guid Id, UpdateWorkOrderRequest Request)
    : IRequest<BaseResponse<bool>>;
