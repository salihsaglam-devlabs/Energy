using Energy.Application.FieldOperations.DailySiteReportMaterial.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialList;

/// <summary>
/// <see cref="GetDailySiteReportMaterialListQuery"/> handler'ı. <see cref="IDailySiteReportMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportMaterialListQueryHandler
    : IRequestHandler<GetDailySiteReportMaterialListQuery, BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>>
{
    private readonly IDailySiteReportMaterialService _service;

    public GetDailySiteReportMaterialListQueryHandler(IDailySiteReportMaterialService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>> Handle(
        GetDailySiteReportMaterialListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
