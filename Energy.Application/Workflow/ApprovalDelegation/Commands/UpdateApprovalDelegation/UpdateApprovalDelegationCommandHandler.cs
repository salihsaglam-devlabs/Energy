using Energy.Application.Workflow.ApprovalDelegation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDelegation.Commands.UpdateApprovalDelegation;

/// <summary>
/// <see cref="UpdateApprovalDelegationCommand"/> handler'ı. <see cref="IApprovalDelegationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateApprovalDelegationCommandHandler
    : IRequestHandler<UpdateApprovalDelegationCommand, BaseResponse<bool>>
{
    private readonly IApprovalDelegationService _service;

    public UpdateApprovalDelegationCommandHandler(IApprovalDelegationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateApprovalDelegationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
