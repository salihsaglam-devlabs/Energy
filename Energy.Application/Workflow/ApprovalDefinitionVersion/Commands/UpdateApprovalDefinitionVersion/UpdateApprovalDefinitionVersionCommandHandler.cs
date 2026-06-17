using Energy.Application.Workflow.ApprovalDefinitionVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Commands.UpdateApprovalDefinitionVersion;

/// <summary>
/// <see cref="UpdateApprovalDefinitionVersionCommand"/> handler'ı. <see cref="IApprovalDefinitionVersionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalDefinitionVersionCommandHandler
    : IRequestHandler<UpdateApprovalDefinitionVersionCommand, BaseResponse<bool>>
{
    private readonly IApprovalDefinitionVersionService _service;

    public UpdateApprovalDefinitionVersionCommandHandler(IApprovalDefinitionVersionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalDefinitionVersionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
