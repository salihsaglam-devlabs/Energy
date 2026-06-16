using Energy.Application.FieldOperations.DailySiteReportEquipment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentList;

/// <summary>
/// <see cref="GetDailySiteReportEquipmentListQuery"/> handler'ı. <see cref="IDailySiteReportEquipmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportEquipmentListQueryHandler
    : IRequestHandler<GetDailySiteReportEquipmentListQuery, BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>>
{
    private readonly IDailySiteReportEquipmentService _service;

    public GetDailySiteReportEquipmentListQueryHandler(IDailySiteReportEquipmentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>> Handle(
        GetDailySiteReportEquipmentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
