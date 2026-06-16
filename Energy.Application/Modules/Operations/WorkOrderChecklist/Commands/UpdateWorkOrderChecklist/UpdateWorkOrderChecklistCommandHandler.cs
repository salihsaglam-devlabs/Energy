using Energy.Application.Modules.Operations.WorkOrderChecklist.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Commands.UpdateWorkOrderChecklist;

/// <summary>
/// <see cref="UpdateWorkOrderChecklistCommand"/> handler'ı. <see cref="IWorkOrderChecklistService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderChecklistCommandHandler
    : IRequestHandler<UpdateWorkOrderChecklistCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderChecklistService _service;

    public UpdateWorkOrderChecklistCommandHandler(IWorkOrderChecklistService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderChecklistCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
