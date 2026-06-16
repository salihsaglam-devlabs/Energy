using Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerList;

/// <summary>
/// <see cref="GetDailySiteReportWorkerListQuery"/> handler'ı. <see cref="IDailySiteReportWorkerService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportWorkerListQueryHandler
    : IRequestHandler<GetDailySiteReportWorkerListQuery, BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>>
{
    private readonly IDailySiteReportWorkerService _service;

    public GetDailySiteReportWorkerListQueryHandler(IDailySiteReportWorkerService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>> Handle(
        GetDailySiteReportWorkerListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
