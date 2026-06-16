using Energy.Application.Workflow.ApprovalRequest.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Queries.GetApprovalRequestLookup;

/// <summary>
/// <see cref="GetApprovalRequestLookupQuery"/> handler'ı. <see cref="IApprovalRequestLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestLookupQueryHandler
    : IRequestHandler<GetApprovalRequestLookupQuery, BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>>
{
    private readonly IApprovalRequestLookupService _lookup;

    public GetApprovalRequestLookupQueryHandler(IApprovalRequestLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>> Handle(
        GetApprovalRequestLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
