using Energy.Application.Modules.Core.Branch.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Queries.GetBranchLookup;

/// <summary>
/// <see cref="GetBranchLookupQuery"/> handler'ı. <see cref="IBranchLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBranchLookupQueryHandler
    : IRequestHandler<GetBranchLookupQuery, BaseResponse<IReadOnlyList<BranchLookupResponse>>>
{
    private readonly IBranchLookupService _lookup;

    public GetBranchLookupQueryHandler(IBranchLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BranchLookupResponse>>> Handle(
        GetBranchLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
