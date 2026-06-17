using Energy.Application.FieldOperations.DailySiteReportMaterial.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialById;

/// <summary>
/// <see cref="GetDailySiteReportMaterialByIdQuery"/> handler'ı. <see cref="IDailySiteReportMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportMaterialByIdQueryHandler
    : IRequestHandler<GetDailySiteReportMaterialByIdQuery, BaseResponse<DailySiteReportMaterialDetailResponse>>
{
    private readonly IDailySiteReportMaterialService _service;

    public GetDailySiteReportMaterialByIdQueryHandler(IDailySiteReportMaterialService service)
        => _service = service;

    public Task<BaseResponse<DailySiteReportMaterialDetailResponse>> Handle(
        GetDailySiteReportMaterialByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
