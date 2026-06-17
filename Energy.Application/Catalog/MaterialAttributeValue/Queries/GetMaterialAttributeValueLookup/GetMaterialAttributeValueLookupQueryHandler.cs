using Energy.Application.Catalog.MaterialAttributeValue.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueLookup;

/// <summary>
/// <see cref="GetMaterialAttributeValueLookupQuery"/> handler'ı. <see cref="IMaterialAttributeValueLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeValueLookupQueryHandler
    : IRequestHandler<GetMaterialAttributeValueLookupQuery, BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>>
{
    private readonly IMaterialAttributeValueLookupService _lookup;

    public GetMaterialAttributeValueLookupQueryHandler(IMaterialAttributeValueLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>> Handle(
        GetMaterialAttributeValueLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
