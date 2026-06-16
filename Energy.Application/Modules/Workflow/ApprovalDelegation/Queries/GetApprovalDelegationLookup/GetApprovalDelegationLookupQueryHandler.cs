using Energy.Application.Modules.Workflow.ApprovalDelegation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationLookup;

/// <summary>
/// <see cref="GetApprovalDelegationLookupQuery"/> handler'ı. <see cref="IApprovalDelegationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDelegationLookupQueryHandler
    : IRequestHandler<GetApprovalDelegationLookupQuery, BaseResponse<IReadOnlyList<ApprovalDelegationLookupResponse>>>
{
    private readonly IApprovalDelegationLookupService _lookup;

    public GetApprovalDelegationLookupQueryHandler(IApprovalDelegationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalDelegationLookupResponse>>> Handle(
        GetApprovalDelegationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
