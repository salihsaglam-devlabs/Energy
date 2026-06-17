using Energy.Application.Catalog.MaterialUnitConversion.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionLookup;

/// <summary>
/// <see cref="GetMaterialUnitConversionLookupQuery"/> handler'ı. <see cref="IMaterialUnitConversionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialUnitConversionLookupQueryHandler
    : IRequestHandler<GetMaterialUnitConversionLookupQuery, BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>>
{
    private readonly IMaterialUnitConversionLookupService _lookup;

    public GetMaterialUnitConversionLookupQueryHandler(IMaterialUnitConversionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>> Handle(
        GetMaterialUnitConversionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
