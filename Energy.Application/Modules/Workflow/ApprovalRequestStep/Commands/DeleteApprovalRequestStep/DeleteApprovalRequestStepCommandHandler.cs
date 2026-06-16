using Energy.Application.Modules.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Commands.DeleteApprovalRequestStep;

/// <summary>
/// <see cref="DeleteApprovalRequestStepCommand"/> handler'ı. <see cref="IApprovalRequestStepService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalRequestStepCommandHandler
    : IRequestHandler<DeleteApprovalRequestStepCommand, BaseResponse<bool>>
{
    private readonly IApprovalRequestStepService _service;

    public DeleteApprovalRequestStepCommandHandler(IApprovalRequestStepService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalRequestStepCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
