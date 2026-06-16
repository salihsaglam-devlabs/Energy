using Energy.Application.Catalog.Brand.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;
using MediatR;

namespace Energy.Application.Catalog.Brand.Queries.GetBrandLookup;

/// <summary>
/// <see cref="GetBrandLookupQuery"/> handler'ı. <see cref="IBrandLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBrandLookupQueryHandler
    : IRequestHandler<GetBrandLookupQuery, BaseResponse<IReadOnlyList<BrandLookupResponse>>>
{
    private readonly IBrandLookupService _lookup;

    public GetBrandLookupQueryHandler(IBrandLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BrandLookupResponse>>> Handle(
        GetBrandLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
