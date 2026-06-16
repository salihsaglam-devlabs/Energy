using Energy.Application.Operations.WorkOrderChecklist.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklist.Commands.CreateWorkOrderChecklist;

/// <summary>
/// <see cref="CreateWorkOrderChecklistCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderChecklistService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderChecklistCommandHandler
    : IRequestHandler<CreateWorkOrderChecklistCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderChecklistService _service;

    public CreateWorkOrderChecklistCommandHandler(IWorkOrderChecklistService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderChecklistCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
