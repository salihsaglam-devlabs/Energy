using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.DeleteApprovalRequestApprover;

/// <summary>
/// <see cref="DeleteApprovalRequestApproverCommand"/> handler'ı. <see cref="IApprovalRequestApproverService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalRequestApproverCommandHandler
    : IRequestHandler<DeleteApprovalRequestApproverCommand, BaseResponse<bool>>
{
    private readonly IApprovalRequestApproverService _service;

    public DeleteApprovalRequestApproverCommandHandler(IApprovalRequestApproverService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalRequestApproverCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
