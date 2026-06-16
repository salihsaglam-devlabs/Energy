using Energy.Application.Modules.Operations.WorkOrderChecklist.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Commands.DeleteWorkOrderChecklist;

/// <summary>
/// <see cref="DeleteWorkOrderChecklistCommand"/> handler'ı. <see cref="IWorkOrderChecklistService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderChecklistCommandHandler
    : IRequestHandler<DeleteWorkOrderChecklistCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderChecklistService _service;

    public DeleteWorkOrderChecklistCommandHandler(IWorkOrderChecklistService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderChecklistCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
