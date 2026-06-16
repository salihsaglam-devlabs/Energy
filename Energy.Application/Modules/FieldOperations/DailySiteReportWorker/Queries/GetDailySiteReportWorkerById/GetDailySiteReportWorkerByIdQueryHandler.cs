using Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerById;

/// <summary>
/// <see cref="GetDailySiteReportWorkerByIdQuery"/> handler'ı. <see cref="IDailySiteReportWorkerService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportWorkerByIdQueryHandler
    : IRequestHandler<GetDailySiteReportWorkerByIdQuery, BaseResponse<DailySiteReportWorkerDetailResponse>>
{
    private readonly IDailySiteReportWorkerService _service;

    public GetDailySiteReportWorkerByIdQueryHandler(IDailySiteReportWorkerService service)
        => _service = service;

    public Task<BaseResponse<DailySiteReportWorkerDetailResponse>> Handle(
        GetDailySiteReportWorkerByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
