using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;
using Energy.Application.HR.Reports.TimesheetSummary.Queries.GetTimesheetSummaryData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.HR.Reports;

/// <summary>TimesheetSummary raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/h-r/reports/timesheet-summary")]
public sealed class TimesheetSummaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetSummaryController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>>> GetData([FromQuery] TimesheetSummaryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetSummaryDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] TimesheetSummaryRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetTimesheetSummaryDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "timesheet-summary.csv");
    }
}
