using Energy.Application.FieldOperations.MeasurementSheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetList;

/// <summary>
/// <see cref="GetMeasurementSheetListQuery"/> handler'ı. <see cref="IMeasurementSheetService"/>'i orkestre eder.
/// </summary>
public sealed class GetMeasurementSheetListQueryHandler
    : IRequestHandler<GetMeasurementSheetListQuery, BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>>
{
    private readonly IMeasurementSheetService _service;

    public GetMeasurementSheetListQueryHandler(IMeasurementSheetService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>> Handle(
        GetMeasurementSheetListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
