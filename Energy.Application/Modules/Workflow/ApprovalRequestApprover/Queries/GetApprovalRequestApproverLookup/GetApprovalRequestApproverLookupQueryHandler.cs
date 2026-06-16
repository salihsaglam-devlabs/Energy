using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverLookup;

/// <summary>
/// <see cref="GetApprovalRequestApproverLookupQuery"/> handler'ı. <see cref="IApprovalRequestApproverLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestApproverLookupQueryHandler
    : IRequestHandler<GetApprovalRequestApproverLookupQuery, BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>>
{
    private readonly IApprovalRequestApproverLookupService _lookup;

    public GetApprovalRequestApproverLookupQueryHandler(IApprovalRequestApproverLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>> Handle(
        GetApprovalRequestApproverLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
