using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.HR.Reports.TimesheetSummary.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;

namespace Energy.Api.Controllers.Modules.HR.Reports;

/// <summary>TimesheetSummary raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/h-r/reports/timesheet-summary")]
public sealed class TimesheetSummaryController : ControllerBase
{
    private readonly ITimesheetSummaryService _service;

    public TimesheetSummaryController(ITimesheetSummaryService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>>> GetData([FromQuery] TimesheetSummaryRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] TimesheetSummaryRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _service.GetDataAsync(request, ct);
        var rows = result.Data?.Items ?? [];
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[] { "TimesheetNo","PeriodStart","PeriodEnd","Status" }));
        foreach (var r in rows)
        {
            sb.AppendLine((r.TimesheetNo ?? string.Empty) + "," + r.PeriodStart.ToString() + "," + r.PeriodEnd.ToString() + "," + (r.Status ?? string.Empty));
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "timesheet-summary.csv");
    }
}
