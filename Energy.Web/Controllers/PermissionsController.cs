using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[Authorize]
[Route("permissions")]
[Route("system/permissions")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class PermissionsController : Controller
{
    private readonly IPermissionApiClient _permissionApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PermissionsController(
        IPermissionApiClient permissionApiClient,
        IStringLocalizer<SharedResource> localizer)
    {
        _permissionApiClient = permissionApiClient;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.PermissionsScreen.Title);
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] GridLoadOptions options, CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.GetPermissionsAsync(options.ToPaginatedRequest(), cancellationToken);
        return envelope.ToGridResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.GetPermissionAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.CreatePermissionAsync(request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.UpdatePermissionAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.DeletePermissionAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("seed-defaults")]
    public async Task<IActionResult> SeedDefaults(CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.SeedDefaultsAsync(cancellationToken);
        return envelope.ToJsonResult();
    }
}

