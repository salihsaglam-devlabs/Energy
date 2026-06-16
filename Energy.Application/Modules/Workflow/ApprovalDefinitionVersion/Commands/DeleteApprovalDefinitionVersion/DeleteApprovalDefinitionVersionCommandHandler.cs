using Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Commands.DeleteApprovalDefinitionVersion;

/// <summary>
/// <see cref="DeleteApprovalDefinitionVersionCommand"/> handler'ı. <see cref="IApprovalDefinitionVersionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalDefinitionVersionCommandHandler
    : IRequestHandler<DeleteApprovalDefinitionVersionCommand, BaseResponse<bool>>
{
    private readonly IApprovalDefinitionVersionService _service;

    public DeleteApprovalDefinitionVersionCommandHandler(IApprovalDefinitionVersionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalDefinitionVersionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
