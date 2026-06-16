using Energy.Application.FieldOperations.DailySiteReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Queries.GetDailySiteReportById;

/// <summary>
/// <see cref="GetDailySiteReportByIdQuery"/> handler'ı. <see cref="IDailySiteReportService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportByIdQueryHandler
    : IRequestHandler<GetDailySiteReportByIdQuery, BaseResponse<DailySiteReportDetailResponse>>
{
    private readonly IDailySiteReportService _service;

    public GetDailySiteReportByIdQueryHandler(IDailySiteReportService service)
        => _service = service;

    public Task<BaseResponse<DailySiteReportDetailResponse>> Handle(
        GetDailySiteReportByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
