using Energy.Application.Workflow.ApprovalRequestApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestApprover.Commands.CreateApprovalRequestApprover;

/// <summary>
/// <see cref="CreateApprovalRequestApproverCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalRequestApproverService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalRequestApproverCommandHandler
    : IRequestHandler<CreateApprovalRequestApproverCommand, BaseResponse<Guid>>
{
    private readonly IApprovalRequestApproverService _service;

    public CreateApprovalRequestApproverCommandHandler(IApprovalRequestApproverService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalRequestApproverCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
