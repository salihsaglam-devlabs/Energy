using Energy.Application.Workflow.ApprovalStepApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Commands.DeleteApprovalStepApprover;

/// <summary>
/// <see cref="DeleteApprovalStepApproverCommand"/> handler'ı. <see cref="IApprovalStepApproverService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalStepApproverCommandHandler
    : IRequestHandler<DeleteApprovalStepApproverCommand, BaseResponse<bool>>
{
    private readonly IApprovalStepApproverService _service;

    public DeleteApprovalStepApproverCommandHandler(IApprovalStepApproverService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalStepApproverCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
