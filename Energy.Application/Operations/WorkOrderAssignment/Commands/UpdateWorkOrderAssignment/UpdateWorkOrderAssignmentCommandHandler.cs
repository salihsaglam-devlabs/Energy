using Energy.Application.Operations.WorkOrderAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderAssignment.Commands.UpdateWorkOrderAssignment;

/// <summary>
/// <see cref="UpdateWorkOrderAssignmentCommand"/> handler'ı. <see cref="IWorkOrderAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderAssignmentCommandHandler
    : IRequestHandler<UpdateWorkOrderAssignmentCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderAssignmentService _service;

    public UpdateWorkOrderAssignmentCommandHandler(IWorkOrderAssignmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
