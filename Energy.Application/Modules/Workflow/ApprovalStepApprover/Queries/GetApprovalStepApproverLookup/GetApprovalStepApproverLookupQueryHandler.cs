using Energy.Application.Modules.Workflow.ApprovalStepApprover.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverLookup;

/// <summary>
/// <see cref="GetApprovalStepApproverLookupQuery"/> handler'ı. <see cref="IApprovalStepApproverLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalStepApproverLookupQueryHandler
    : IRequestHandler<GetApprovalStepApproverLookupQuery, BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>>
{
    private readonly IApprovalStepApproverLookupService _lookup;

    public GetApprovalStepApproverLookupQueryHandler(IApprovalStepApproverLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>> Handle(
        GetApprovalStepApproverLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
