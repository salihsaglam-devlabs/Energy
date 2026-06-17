using Energy.Application.FieldOperations.MeasurementSheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetById;

/// <summary>
/// <see cref="GetMeasurementSheetByIdQuery"/> handler'ı. <see cref="IMeasurementSheetService"/>'i orkestre eder.
/// </summary>
public sealed class GetMeasurementSheetByIdQueryHandler
    : IRequestHandler<GetMeasurementSheetByIdQuery, BaseResponse<MeasurementSheetDetailResponse>>
{
    private readonly IMeasurementSheetService _service;

    public GetMeasurementSheetByIdQueryHandler(IMeasurementSheetService service)
        => _service = service;

    public Task<BaseResponse<MeasurementSheetDetailResponse>> Handle(
        GetMeasurementSheetByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
