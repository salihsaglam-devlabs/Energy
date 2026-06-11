using Asp.Versioning;
using Energy.Application.Home.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/home")]
public sealed class HomeController : ControllerBase
{
    private readonly IHomeService _home;
    public HomeController(IHomeService home) { _home = home; }

    [HttpGet("dashboard")]
    public async Task<ActionResult<BaseResponse<HomeDashboardResponse>>> GetDashboard([FromQuery] GetHomeDashboardRequest request, CancellationToken ct)
        => Ok(BaseResponse<HomeDashboardResponse>.Success(await _home.GetDashboardAsync(request, ct)));
}
