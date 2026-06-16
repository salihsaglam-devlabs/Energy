using Energy.Application.Modules.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Commands.CreateApprovalRequestStep;

/// <summary>
/// <see cref="CreateApprovalRequestStepCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalRequestStepService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalRequestStepCommandHandler
    : IRequestHandler<CreateApprovalRequestStepCommand, BaseResponse<Guid>>
{
    private readonly IApprovalRequestStepService _service;

    public CreateApprovalRequestStepCommandHandler(IApprovalRequestStepService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalRequestStepCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
