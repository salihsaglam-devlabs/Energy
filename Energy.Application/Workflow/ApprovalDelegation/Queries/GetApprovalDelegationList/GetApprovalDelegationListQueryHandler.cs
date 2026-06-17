using Energy.Application.Workflow.ApprovalDelegation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationList;

/// <summary>
/// <see cref="GetApprovalDelegationListQuery"/> handler'ı. <see cref="IApprovalDelegationService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDelegationListQueryHandler
    : IRequestHandler<GetApprovalDelegationListQuery, BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>>
{
    private readonly IApprovalDelegationService _service;

    public GetApprovalDelegationListQueryHandler(IApprovalDelegationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>> Handle(
        GetApprovalDelegationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
