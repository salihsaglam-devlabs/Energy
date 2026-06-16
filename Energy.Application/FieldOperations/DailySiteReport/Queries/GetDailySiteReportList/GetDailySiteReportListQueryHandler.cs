using Energy.Application.FieldOperations.DailySiteReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Queries.GetDailySiteReportList;

/// <summary>
/// <see cref="GetDailySiteReportListQuery"/> handler'ı. <see cref="IDailySiteReportService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportListQueryHandler
    : IRequestHandler<GetDailySiteReportListQuery, BaseResponse<PaginatedResponse<DailySiteReportListResponse>>>
{
    private readonly IDailySiteReportService _service;

    public GetDailySiteReportListQueryHandler(IDailySiteReportService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>> Handle(
        GetDailySiteReportListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
