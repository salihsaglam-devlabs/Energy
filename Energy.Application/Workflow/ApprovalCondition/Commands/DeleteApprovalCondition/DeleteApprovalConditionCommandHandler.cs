using Energy.Application.Workflow.ApprovalCondition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalCondition.Commands.DeleteApprovalCondition;

/// <summary>
/// <see cref="DeleteApprovalConditionCommand"/> handler'ı. <see cref="IApprovalConditionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalConditionCommandHandler
    : IRequestHandler<DeleteApprovalConditionCommand, BaseResponse<bool>>
{
    private readonly IApprovalConditionService _service;

    public DeleteApprovalConditionCommandHandler(IApprovalConditionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalConditionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
