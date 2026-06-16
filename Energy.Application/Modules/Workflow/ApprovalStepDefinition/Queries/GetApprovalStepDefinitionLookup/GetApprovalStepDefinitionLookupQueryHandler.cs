using Energy.Application.Modules.Workflow.ApprovalStepDefinition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionLookup;

/// <summary>
/// <see cref="GetApprovalStepDefinitionLookupQuery"/> handler'ı. <see cref="IApprovalStepDefinitionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalStepDefinitionLookupQueryHandler
    : IRequestHandler<GetApprovalStepDefinitionLookupQuery, BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>>
{
    private readonly IApprovalStepDefinitionLookupService _lookup;

    public GetApprovalStepDefinitionLookupQueryHandler(IApprovalStepDefinitionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>> Handle(
        GetApprovalStepDefinitionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
