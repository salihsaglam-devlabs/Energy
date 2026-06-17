using Energy.Application.Workflow.ApprovalAction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalAction.Commands.CreateApprovalAction;

/// <summary>
/// <see cref="CreateApprovalActionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalActionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalActionCommandHandler
    : IRequestHandler<CreateApprovalActionCommand, BaseResponse<Guid>>
{
    private readonly IApprovalActionService _service;

    public CreateApprovalActionCommandHandler(IApprovalActionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalActionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
