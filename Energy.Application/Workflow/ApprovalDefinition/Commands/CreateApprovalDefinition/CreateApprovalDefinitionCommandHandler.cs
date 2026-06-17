using Energy.Application.Workflow.ApprovalDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinition.Commands.CreateApprovalDefinition;

/// <summary>
/// <see cref="CreateApprovalDefinitionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalDefinitionCommandHandler
    : IRequestHandler<CreateApprovalDefinitionCommand, BaseResponse<Guid>>
{
    private readonly IApprovalDefinitionService _service;

    public CreateApprovalDefinitionCommandHandler(IApprovalDefinitionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
