using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;

namespace Energy.Api.Controllers.Modules.Projects.Reports;

/// <summary>ProjectStatusReport raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/reports/project-status-report")]
public sealed class ProjectStatusReportController : ControllerBase
{
    private readonly IProjectStatusReportService _service;

    public ProjectStatusReportController(IProjectStatusReportService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>>> GetData([FromQuery] ProjectStatusReportRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] ProjectStatusReportRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _service.GetDataAsync(request, ct);
        var rows = result.Data?.Items ?? [];
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[] { "Code","Name","ProjectTypeId","StatusId","StartDate","EndDate" }));
        foreach (var r in rows)
        {
            sb.AppendLine((r.Code ?? string.Empty) + "," + (r.Name ?? string.Empty) + "," + r.ProjectTypeId.ToString() + "," + r.StatusId.ToString() + "," + r.StartDate.ToString() + "," + r.EndDate.ToString());
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "project-status-report.csv");
    }
}
