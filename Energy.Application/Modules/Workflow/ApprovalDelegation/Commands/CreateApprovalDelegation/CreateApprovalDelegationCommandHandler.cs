using Energy.Application.Modules.Workflow.ApprovalDelegation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.CreateApprovalDelegation;

/// <summary>
/// <see cref="CreateApprovalDelegationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IApprovalDelegationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateApprovalDelegationCommandHandler
    : IRequestHandler<CreateApprovalDelegationCommand, BaseResponse<Guid>>
{
    private readonly IApprovalDelegationService _service;

    public CreateApprovalDelegationCommandHandler(IApprovalDelegationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateApprovalDelegationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
