using Energy.Application.Modules.Workflow.ApprovalCondition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Commands.UpdateApprovalCondition;

/// <summary>
/// <see cref="UpdateApprovalConditionCommand"/> handler'ı. <see cref="IApprovalConditionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalConditionCommandHandler
    : IRequestHandler<UpdateApprovalConditionCommand, BaseResponse<bool>>
{
    private readonly IApprovalConditionService _service;

    public UpdateApprovalConditionCommandHandler(IApprovalConditionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalConditionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
