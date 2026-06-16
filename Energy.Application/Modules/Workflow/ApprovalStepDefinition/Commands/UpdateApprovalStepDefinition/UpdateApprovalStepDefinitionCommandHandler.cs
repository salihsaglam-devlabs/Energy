using Energy.Application.Modules.Workflow.ApprovalStepDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Commands.UpdateApprovalStepDefinition;

/// <summary>
/// <see cref="UpdateApprovalStepDefinitionCommand"/> handler'ı. <see cref="IApprovalStepDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalStepDefinitionCommandHandler
    : IRequestHandler<UpdateApprovalStepDefinitionCommand, BaseResponse<bool>>
{
    private readonly IApprovalStepDefinitionService _service;

    public UpdateApprovalStepDefinitionCommandHandler(IApprovalStepDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalStepDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
