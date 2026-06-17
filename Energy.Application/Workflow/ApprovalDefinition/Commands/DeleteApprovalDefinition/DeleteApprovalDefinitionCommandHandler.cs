using Energy.Application.Workflow.ApprovalDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinition.Commands.DeleteApprovalDefinition;

/// <summary>
/// <see cref="DeleteApprovalDefinitionCommand"/> handler'ı. <see cref="IApprovalDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalDefinitionCommandHandler
    : IRequestHandler<DeleteApprovalDefinitionCommand, BaseResponse<bool>>
{
    private readonly IApprovalDefinitionService _service;

    public DeleteApprovalDefinitionCommandHandler(IApprovalDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
