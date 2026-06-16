using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;
using Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Queries.GetProjectStatusReportData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.Projects.Reports;

/// <summary>ProjectStatusReport raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/reports/project-status-report")]
public sealed class ProjectStatusReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectStatusReportController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>>> GetData([FromQuery] ProjectStatusReportRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectStatusReportDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] ProjectStatusReportRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetProjectStatusReportDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "project-status-report.csv");
    }
}
