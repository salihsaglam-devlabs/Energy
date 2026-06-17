using Energy.Application.Workflow.ApprovalStepDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Commands.DeleteApprovalStepDefinition;

/// <summary>
/// <see cref="DeleteApprovalStepDefinitionCommand"/> handler'ı. <see cref="IApprovalStepDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalStepDefinitionCommandHandler
    : IRequestHandler<DeleteApprovalStepDefinitionCommand, BaseResponse<bool>>
{
    private readonly IApprovalStepDefinitionService _service;

    public DeleteApprovalStepDefinitionCommandHandler(IApprovalStepDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalStepDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
