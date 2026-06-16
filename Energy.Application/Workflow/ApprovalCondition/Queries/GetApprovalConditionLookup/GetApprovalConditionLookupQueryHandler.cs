using Energy.Application.Workflow.ApprovalCondition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalCondition.Queries.GetApprovalConditionLookup;

/// <summary>
/// <see cref="GetApprovalConditionLookupQuery"/> handler'ı. <see cref="IApprovalConditionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalConditionLookupQueryHandler
    : IRequestHandler<GetApprovalConditionLookupQuery, BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>>
{
    private readonly IApprovalConditionLookupService _lookup;

    public GetApprovalConditionLookupQueryHandler(IApprovalConditionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>> Handle(
        GetApprovalConditionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
