using Energy.Application.Modules.Workflow.ApprovalStepApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Commands.CreateApprovalStepApprover;

/// <summary>
/// <see cref="CreateApprovalStepApproverCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalStepApproverService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalStepApproverCommandHandler
    : IRequestHandler<CreateApprovalStepApproverCommand, BaseResponse<Guid>>
{
    private readonly IApprovalStepApproverService _service;

    public CreateApprovalStepApproverCommandHandler(IApprovalStepApproverService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalStepApproverCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
