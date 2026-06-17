using Energy.Application.Workflow.ApprovalDefinition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionLookup;

/// <summary>
/// <see cref="GetApprovalDefinitionLookupQuery"/> handler'ı. <see cref="IApprovalDefinitionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDefinitionLookupQueryHandler
    : IRequestHandler<GetApprovalDefinitionLookupQuery, BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>>
{
    private readonly IApprovalDefinitionLookupService _lookup;

    public GetApprovalDefinitionLookupQueryHandler(IApprovalDefinitionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>> Handle(
        GetApprovalDefinitionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
