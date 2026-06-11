using Asp.Versioning;
using Energy.Application.Home.Queries.GetHomeDashboard;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/home")]
[Authorize]
public sealed class HomeController : ControllerBase
{
    private readonly ISender _sender;

    public HomeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("dashboard")]
    [Authorize(Policy = HomePermissions.GetDashboard)]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] GetHomeDashboardRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetHomeDashboardQuery(request.IncludeQuickLinks, request.QuickLinkCount);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }
}
