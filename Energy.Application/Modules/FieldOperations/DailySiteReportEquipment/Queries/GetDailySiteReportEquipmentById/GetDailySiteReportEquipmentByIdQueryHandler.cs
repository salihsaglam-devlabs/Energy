using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentById;

/// <summary>
/// <see cref="GetDailySiteReportEquipmentByIdQuery"/> handler'ı. <see cref="IDailySiteReportEquipmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportEquipmentByIdQueryHandler
    : IRequestHandler<GetDailySiteReportEquipmentByIdQuery, BaseResponse<DailySiteReportEquipmentDetailResponse>>
{
    private readonly IDailySiteReportEquipmentService _service;

    public GetDailySiteReportEquipmentByIdQueryHandler(IDailySiteReportEquipmentService service)
        => _service = service;

    public Task<BaseResponse<DailySiteReportEquipmentDetailResponse>> Handle(
        GetDailySiteReportEquipmentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
