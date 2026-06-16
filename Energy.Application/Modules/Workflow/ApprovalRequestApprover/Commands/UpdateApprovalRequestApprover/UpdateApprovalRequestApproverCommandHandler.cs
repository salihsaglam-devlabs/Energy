using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.UpdateApprovalRequestApprover;

/// <summary>
/// <see cref="UpdateApprovalRequestApproverCommand"/> handler'ı. <see cref="IApprovalRequestApproverService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalRequestApproverCommandHandler
    : IRequestHandler<UpdateApprovalRequestApproverCommand, BaseResponse<bool>>
{
    private readonly IApprovalRequestApproverService _service;

    public UpdateApprovalRequestApproverCommandHandler(IApprovalRequestApproverService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalRequestApproverCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
