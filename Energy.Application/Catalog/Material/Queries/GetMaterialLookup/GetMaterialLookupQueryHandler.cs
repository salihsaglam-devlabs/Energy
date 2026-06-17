using Energy.Application.Catalog.Material.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Queries.GetMaterialLookup;

/// <summary>
/// <see cref="GetMaterialLookupQuery"/> handler'ı. <see cref="IMaterialLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialLookupQueryHandler
    : IRequestHandler<GetMaterialLookupQuery, BaseResponse<IReadOnlyList<MaterialLookupResponse>>>
{
    private readonly IMaterialLookupService _lookup;

    public GetMaterialLookupQueryHandler(IMaterialLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialLookupResponse>>> Handle(
        GetMaterialLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
