using Energy.Application.Modules.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Commands.UpdateApprovalRequestStep;

/// <summary>
/// <see cref="UpdateApprovalRequestStepCommand"/> handler'ı. <see cref="IApprovalRequestStepService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalRequestStepCommandHandler
    : IRequestHandler<UpdateApprovalRequestStepCommand, BaseResponse<bool>>
{
    private readonly IApprovalRequestStepService _service;

    public UpdateApprovalRequestStepCommandHandler(IApprovalRequestStepService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalRequestStepCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
