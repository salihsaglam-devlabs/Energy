using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Energy.Application.Modules.Home.Dashboard.Queries.GetEnterpriseMetrics;
using Energy.Application.Modules.Home.Dashboard.Queries.GetHomeDashboard;

namespace Energy.Api.Controllers.Home;

/// <summary>Ana sayfa/dashboard uç noktaları (kurumsal metrikler).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/home")]
public sealed class HomeController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("dashboard")]
    public async Task<ActionResult<BaseResponse<HomeDashboardResponse>>> GetDashboard([FromQuery] GetHomeDashboardRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetHomeDashboardQuery(request), ct));

    [HttpGet("enterprise-metrics")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EnterpriseMetricResponse>>>> EnterpriseMetrics(CancellationToken ct)
        => Ok(await _mediator.Send(new GetEnterpriseMetricsQuery(), ct));
}
