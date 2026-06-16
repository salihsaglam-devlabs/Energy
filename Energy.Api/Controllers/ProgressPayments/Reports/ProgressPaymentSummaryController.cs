using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;
using Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Queries.GetProgressPaymentSummaryData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.ProgressPayments.Reports;

/// <summary>ProgressPaymentSummary raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/progress-payments/reports/progress-payment-summary")]
public sealed class ProgressPaymentSummaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressPaymentSummaryController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>>> GetData([FromQuery] ProgressPaymentSummaryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentSummaryDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] ProgressPaymentSummaryRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetProgressPaymentSummaryDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "progress-payment-summary.csv");
    }
}
