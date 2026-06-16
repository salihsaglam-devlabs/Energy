using Energy.Application.Modules.Workflow.ApprovalAction.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Queries.GetApprovalActionLookup;

/// <summary>
/// <see cref="GetApprovalActionLookupQuery"/> handler'ı. <see cref="IApprovalActionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalActionLookupQueryHandler
    : IRequestHandler<GetApprovalActionLookupQuery, BaseResponse<IReadOnlyList<ApprovalActionLookupResponse>>>
{
    private readonly IApprovalActionLookupService _lookup;

    public GetApprovalActionLookupQueryHandler(IApprovalActionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalActionLookupResponse>>> Handle(
        GetApprovalActionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
