using Energy.Application.Workflow.ApprovalStepDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Commands.CreateApprovalStepDefinition;

/// <summary>
/// <see cref="CreateApprovalStepDefinitionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalStepDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalStepDefinitionCommandHandler
    : IRequestHandler<CreateApprovalStepDefinitionCommand, BaseResponse<Guid>>
{
    private readonly IApprovalStepDefinitionService _service;

    public CreateApprovalStepDefinitionCommandHandler(IApprovalStepDefinitionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalStepDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
