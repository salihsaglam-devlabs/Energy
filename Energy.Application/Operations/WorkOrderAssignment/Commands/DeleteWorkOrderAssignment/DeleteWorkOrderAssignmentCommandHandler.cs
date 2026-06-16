using Energy.Application.Operations.WorkOrderAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderAssignment.Commands.DeleteWorkOrderAssignment;

/// <summary>
/// <see cref="DeleteWorkOrderAssignmentCommand"/> handler'ı. <see cref="IWorkOrderAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderAssignmentCommandHandler
    : IRequestHandler<DeleteWorkOrderAssignmentCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderAssignmentService _service;

    public DeleteWorkOrderAssignmentCommandHandler(IWorkOrderAssignmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
