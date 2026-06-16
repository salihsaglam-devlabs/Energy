using Energy.Application.Modules.Operations.WorkOrderAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Commands.CreateWorkOrderAssignment;

/// <summary>
/// <see cref="CreateWorkOrderAssignmentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderAssignmentCommandHandler
    : IRequestHandler<CreateWorkOrderAssignmentCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderAssignmentService _service;

    public CreateWorkOrderAssignmentCommandHandler(IWorkOrderAssignmentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
