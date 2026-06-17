using Energy.Application.Workflow.ApprovalDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinition.Commands.UpdateApprovalDefinition;

/// <summary>
/// <see cref="UpdateApprovalDefinitionCommand"/> handler'ı. <see cref="IApprovalDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalDefinitionCommandHandler
    : IRequestHandler<UpdateApprovalDefinitionCommand, BaseResponse<bool>>
{
    private readonly IApprovalDefinitionService _service;

    public UpdateApprovalDefinitionCommandHandler(IApprovalDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
