using Energy.Application.Modules.Core.UnitConversion.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Queries.GetUnitConversionLookup;

/// <summary>
/// <see cref="GetUnitConversionLookupQuery"/> handler'ı. <see cref="IUnitConversionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetUnitConversionLookupQueryHandler
    : IRequestHandler<GetUnitConversionLookupQuery, BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>>
{
    private readonly IUnitConversionLookupService _lookup;

    public GetUnitConversionLookupQueryHandler(IUnitConversionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>> Handle(
        GetUnitConversionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
