using Energy.Application.Workflow.ApprovalDefinitionVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Commands.CreateApprovalDefinitionVersion;

/// <summary>
/// <see cref="CreateApprovalDefinitionVersionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalDefinitionVersionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalDefinitionVersionCommandHandler
    : IRequestHandler<CreateApprovalDefinitionVersionCommand, BaseResponse<Guid>>
{
    private readonly IApprovalDefinitionVersionService _service;

    public CreateApprovalDefinitionVersionCommandHandler(IApprovalDefinitionVersionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalDefinitionVersionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
