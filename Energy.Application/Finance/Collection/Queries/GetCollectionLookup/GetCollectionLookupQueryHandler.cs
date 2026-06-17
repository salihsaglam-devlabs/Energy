using Energy.Application.Finance.Collection.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Queries.GetCollectionLookup;

/// <summary>
/// <see cref="GetCollectionLookupQuery"/> handler'ı. <see cref="ICollectionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetCollectionLookupQueryHandler
    : IRequestHandler<GetCollectionLookupQuery, BaseResponse<IReadOnlyList<CollectionLookupResponse>>>
{
    private readonly ICollectionLookupService _lookup;

    public GetCollectionLookupQueryHandler(ICollectionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<CollectionLookupResponse>>> Handle(
        GetCollectionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
