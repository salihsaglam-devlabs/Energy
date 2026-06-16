using Energy.Application.FieldOperations.MeasurementSheet.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetLookup;

/// <summary>
/// <see cref="GetMeasurementSheetLookupQuery"/> handler'ı. <see cref="IMeasurementSheetLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMeasurementSheetLookupQueryHandler
    : IRequestHandler<GetMeasurementSheetLookupQuery, BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>>
{
    private readonly IMeasurementSheetLookupService _lookup;

    public GetMeasurementSheetLookupQueryHandler(IMeasurementSheetLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>> Handle(
        GetMeasurementSheetLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
