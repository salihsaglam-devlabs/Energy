using Energy.Application.Modules.Workflow.ApprovalDelegation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationById;

/// <summary>
/// <see cref="GetApprovalDelegationByIdQuery"/> handler'ı. <see cref="IApprovalDelegationService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDelegationByIdQueryHandler
    : IRequestHandler<GetApprovalDelegationByIdQuery, BaseResponse<ApprovalDelegationDetailResponse>>
{
    private readonly IApprovalDelegationService _service;

    public GetApprovalDelegationByIdQueryHandler(IApprovalDelegationService service)
        => _service = service;

    public Task<BaseResponse<ApprovalDelegationDetailResponse>> Handle(
        GetApprovalDelegationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
