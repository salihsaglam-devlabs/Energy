using Energy.Application.Modules.Workflow.ApprovalRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Commands.DeleteApprovalRequest;

/// <summary>
/// <see cref="DeleteApprovalRequestCommand"/> handler'ı. <see cref="IApprovalRequestService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalRequestCommandHandler
    : IRequestHandler<DeleteApprovalRequestCommand, BaseResponse<bool>>
{
    private readonly IApprovalRequestService _service;

    public DeleteApprovalRequestCommandHandler(IApprovalRequestService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalRequestCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
