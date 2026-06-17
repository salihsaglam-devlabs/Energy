using Energy.Application.Finance.CollectionAllocation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Queries.GetCollectionAllocationLookup;

/// <summary>
/// <see cref="GetCollectionAllocationLookupQuery"/> handler'ı. <see cref="ICollectionAllocationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetCollectionAllocationLookupQueryHandler
    : IRequestHandler<GetCollectionAllocationLookupQuery, BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>>
{
    private readonly ICollectionAllocationLookupService _lookup;

    public GetCollectionAllocationLookupQueryHandler(ICollectionAllocationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>> Handle(
        GetCollectionAllocationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
