using Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionLookup;

/// <summary>
/// <see cref="GetApprovalDefinitionVersionLookupQuery"/> handler'ı. <see cref="IApprovalDefinitionVersionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDefinitionVersionLookupQueryHandler
    : IRequestHandler<GetApprovalDefinitionVersionLookupQuery, BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>>
{
    private readonly IApprovalDefinitionVersionLookupService _lookup;

    public GetApprovalDefinitionVersionLookupQueryHandler(IApprovalDefinitionVersionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>> Handle(
        GetApprovalDefinitionVersionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
