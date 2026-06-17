using Energy.Application.Workflow.ApprovalRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Commands.CreateApprovalRequest;

/// <summary>
/// <see cref="CreateApprovalRequestCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalRequestService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalRequestCommandHandler
    : IRequestHandler<CreateApprovalRequestCommand, BaseResponse<Guid>>
{
    private readonly IApprovalRequestService _service;

    public CreateApprovalRequestCommandHandler(IApprovalRequestService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalRequestCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
