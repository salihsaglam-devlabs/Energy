using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineList;

/// <summary>
/// <see cref="GetMeasurementSheetLineListQuery"/> handler'ı. <see cref="IMeasurementSheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetMeasurementSheetLineListQueryHandler
    : IRequestHandler<GetMeasurementSheetLineListQuery, BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>>
{
    private readonly IMeasurementSheetLineService _service;

    public GetMeasurementSheetLineListQueryHandler(IMeasurementSheetLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>> Handle(
        GetMeasurementSheetLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
