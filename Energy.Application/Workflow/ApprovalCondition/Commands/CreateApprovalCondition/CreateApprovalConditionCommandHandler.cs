using Energy.Application.Workflow.ApprovalCondition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalCondition.Commands.CreateApprovalCondition;

/// <summary>
/// <see cref="CreateApprovalConditionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalConditionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalConditionCommandHandler
    : IRequestHandler<CreateApprovalConditionCommand, BaseResponse<Guid>>
{
    private readonly IApprovalConditionService _service;

    public CreateApprovalConditionCommandHandler(IApprovalConditionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalConditionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
