using Energy.Application.Modules.Workflow.ApprovalDelegation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.DeleteApprovalDelegation;

/// <summary>
/// <see cref="DeleteApprovalDelegationCommand"/> handler'ı. <see cref="IApprovalDelegationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteApprovalDelegationCommandHandler
    : IRequestHandler<DeleteApprovalDelegationCommand, BaseResponse<bool>>
{
    private readonly IApprovalDelegationService _service;

    public DeleteApprovalDelegationCommandHandler(IApprovalDelegationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteApprovalDelegationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
