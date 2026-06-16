using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Web.Clients.Modules.HR.Reports.TimesheetSummary;

namespace Energy.Web.Controllers.Modules.HR.Reports;

/// <summary>TimesheetSummary rapor ekran denetleyicisi (yalnızca API istemcisiyle konuşur, salt-okunur).</summary>
[Authorize]
[Route("h-r/reports/timesheet-summary")]
public sealed class TimesheetSummaryController : Controller
{
    private readonly ITimesheetSummaryApiClient _api;

    public TimesheetSummaryController(ITimesheetSummaryApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Modules/HR/Reports/TimesheetSummary/Index.cshtml");

    [HttpGet("data")]
    public async Task<IActionResult> Data(int skip = 0, int take = 50, DateTime? startDate = null, DateTime? endDate = null, string? status = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var pageSize = take <= 0 ? 50 : take;
        var parts = new List<string> { $"PageNumber={pageNumber}", $"PageSize={pageSize}" };
        if (startDate.HasValue) parts.Add($"StartDate={startDate.Value:O}");
        if (endDate.HasValue) parts.Add($"EndDate={endDate.Value:O}");
        if (!string.IsNullOrWhiteSpace(status)) parts.Add($"Status={Uri.EscapeDataString(status)}");
        var envelope = await _api.GetDataAsync(string.Join("&", parts), ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses.TimesheetSummaryRowResponse>(), totalCount = page?.TotalCount ?? 0 });
    }
}
