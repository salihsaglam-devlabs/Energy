using Energy.Application.Modules.Workflow.ApprovalAction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Commands.UpdateApprovalAction;

/// <summary>
/// <see cref="UpdateApprovalActionCommand"/> handler'ı. <see cref="IApprovalActionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalActionCommandHandler
    : IRequestHandler<UpdateApprovalActionCommand, BaseResponse<bool>>
{
    private readonly IApprovalActionService _service;

    public UpdateApprovalActionCommandHandler(IApprovalActionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalActionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
