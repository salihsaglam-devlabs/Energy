using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Commands.DeleteWorkOrderAssignment;

/// <summary>WorkOrderAssignment kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderAssignmentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
