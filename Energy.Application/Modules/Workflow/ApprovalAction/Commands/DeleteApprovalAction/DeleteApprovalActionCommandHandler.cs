using Energy.Application.Modules.Workflow.ApprovalAction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Commands.DeleteApprovalAction;

/// <summary>
/// <see cref="DeleteApprovalActionCommand"/> handler'ı. <see cref="IApprovalActionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalActionCommandHandler
    : IRequestHandler<DeleteApprovalActionCommand, BaseResponse<bool>>
{
    private readonly IApprovalActionService _service;

    public DeleteApprovalActionCommandHandler(IApprovalActionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalActionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
