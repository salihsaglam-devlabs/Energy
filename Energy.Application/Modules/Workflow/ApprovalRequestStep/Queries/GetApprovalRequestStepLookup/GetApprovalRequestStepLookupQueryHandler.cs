using Energy.Application.Modules.Workflow.ApprovalRequestStep.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepLookup;

/// <summary>
/// <see cref="GetApprovalRequestStepLookupQuery"/> handler'ı. <see cref="IApprovalRequestStepLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestStepLookupQueryHandler
    : IRequestHandler<GetApprovalRequestStepLookupQuery, BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>>
{
    private readonly IApprovalRequestStepLookupService _lookup;

    public GetApprovalRequestStepLookupQueryHandler(IApprovalRequestStepLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>> Handle(
        GetApprovalRequestStepLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
