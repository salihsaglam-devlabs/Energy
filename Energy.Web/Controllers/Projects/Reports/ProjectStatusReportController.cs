using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Web.Clients.Projects.Reports.ProjectStatusReport;

namespace Energy.Web.Controllers.Projects.Reports;

/// <summary>ProjectStatusReport rapor ekran denetleyicisi (yalnızca API istemcisiyle konuşur, salt-okunur).</summary>
[Authorize]
[Route("projects/reports/project-status-report")]
public sealed class ProjectStatusReportController : Controller
{
    private readonly IProjectStatusReportApiClient _api;

    public ProjectStatusReportController(IProjectStatusReportApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Projects/Reports/ProjectStatusReport/Index.cshtml");

    [HttpGet("data")]
    public async Task<IActionResult> Data(int skip = 0, int take = 50, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var pageSize = take <= 0 ? 50 : take;
        var parts = new List<string> { $"PageNumber={pageNumber}", $"PageSize={pageSize}" };
        if (startDate.HasValue) parts.Add($"StartDate={startDate.Value:O}");
        if (endDate.HasValue) parts.Add($"EndDate={endDate.Value:O}");

        var envelope = await _api.GetDataAsync(string.Join("&", parts), ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses.ProjectStatusReportRowResponse>(), totalCount = page?.TotalCount ?? 0 });
    }
}
