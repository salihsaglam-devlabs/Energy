using Energy.Application.FieldOperations.MeasurementSheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineById;

/// <summary>
/// <see cref="GetMeasurementSheetLineByIdQuery"/> handler'ı. <see cref="IMeasurementSheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetMeasurementSheetLineByIdQueryHandler
    : IRequestHandler<GetMeasurementSheetLineByIdQuery, BaseResponse<MeasurementSheetLineDetailResponse>>
{
    private readonly IMeasurementSheetLineService _service;

    public GetMeasurementSheetLineByIdQueryHandler(IMeasurementSheetLineService service)
        => _service = service;

    public Task<BaseResponse<MeasurementSheetLineDetailResponse>> Handle(
        GetMeasurementSheetLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
