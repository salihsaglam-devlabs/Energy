using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineLookup;

/// <summary>
/// <see cref="GetMeasurementSheetLineLookupQuery"/> handler'ı. <see cref="IMeasurementSheetLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMeasurementSheetLineLookupQueryHandler
    : IRequestHandler<GetMeasurementSheetLineLookupQuery, BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>>
{
    private readonly IMeasurementSheetLineLookupService _lookup;

    public GetMeasurementSheetLineLookupQueryHandler(IMeasurementSheetLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>> Handle(
        GetMeasurementSheetLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
