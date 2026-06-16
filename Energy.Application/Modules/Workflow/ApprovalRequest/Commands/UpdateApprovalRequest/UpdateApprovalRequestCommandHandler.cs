using Energy.Application.Modules.Workflow.ApprovalRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Commands.UpdateApprovalRequest;

/// <summary>
/// <see cref="UpdateApprovalRequestCommand"/> handler'ı. <see cref="IApprovalRequestService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalRequestCommandHandler
    : IRequestHandler<UpdateApprovalRequestCommand, BaseResponse<bool>>
{
    private readonly IApprovalRequestService _service;

    public UpdateApprovalRequestCommandHandler(IApprovalRequestService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalRequestCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
