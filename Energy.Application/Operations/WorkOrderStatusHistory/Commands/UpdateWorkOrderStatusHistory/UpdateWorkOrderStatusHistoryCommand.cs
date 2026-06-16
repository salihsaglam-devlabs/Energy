using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Commands.UpdateWorkOrderStatusHistory;

/// <summary>Var olan WorkOrderStatusHistory kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderStatusHistoryCommand(Guid Id, UpdateWorkOrderStatusHistoryRequest Request)
    : IRequest<BaseResponse<bool>>;
