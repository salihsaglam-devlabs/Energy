using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderAssignment.Commands.UpdateWorkOrderAssignment;

/// <summary>Var olan WorkOrderAssignment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderAssignmentCommand(Guid Id, UpdateWorkOrderAssignmentRequest Request)
    : IRequest<BaseResponse<bool>>;
