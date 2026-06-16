using Energy.Application.Core.UnitOfMeasure.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using MediatR;

namespace Energy.Application.Core.UnitOfMeasure.Queries.GetUnitOfMeasureLookup;

/// <summary>
/// <see cref="GetUnitOfMeasureLookupQuery"/> handler'ı. <see cref="IUnitOfMeasureLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetUnitOfMeasureLookupQueryHandler
    : IRequestHandler<GetUnitOfMeasureLookupQuery, BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>>
{
    private readonly IUnitOfMeasureLookupService _lookup;

    public GetUnitOfMeasureLookupQueryHandler(IUnitOfMeasureLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>> Handle(
        GetUnitOfMeasureLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
