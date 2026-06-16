using Energy.Application.Workflow.ApprovalStepApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Commands.UpdateApprovalStepApprover;

/// <summary>
/// <see cref="UpdateApprovalStepApproverCommand"/> handler'ı. <see cref="IApprovalStepApproverService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalStepApproverCommandHandler
    : IRequestHandler<UpdateApprovalStepApproverCommand, BaseResponse<bool>>
{
    private readonly IApprovalStepApproverService _service;

    public UpdateApprovalStepApproverCommandHandler(IApprovalStepApproverService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalStepApproverCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
