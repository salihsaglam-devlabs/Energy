using Energy.Localization;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.System;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[Authorize]
[Route("menus")]
[Route("system/menus")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class MenusController : Controller
{
    private readonly IMenuApiClient _menuApiClient;
    private readonly IPermissionApiClient _permissionApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public MenusController(
        IMenuApiClient menuApiClient,
        IPermissionApiClient permissionApiClient,
        IStringLocalizer<SharedResource> localizer)
    {
        _menuApiClient = menuApiClient;
        _permissionApiClient = permissionApiClient;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.MenusScreen.Title);
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] GridLoadOptions options, CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.GetMenusAsync(options.ToPaginatedRequest(), cancellationToken);
        return envelope.ToGridResult();
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.GetMenusAsync(
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(Array.Empty<object>());
        }

        return Ok(envelope.Data.Items.Select(m => new
        {
            id = m.Id,
            parentId = m.ParentId,
            name = string.IsNullOrEmpty(m.Name) ? m.NameKey : m.Name
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.GetMenuAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateMenuRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.CreateMenuAsync(request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMenuRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.UpdateMenuAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.DeleteMenuAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpGet("permissions-lookup")]
    public async Task<IActionResult> PermissionsLookup(CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.GetPermissionsAsync(
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(Array.Empty<object>());
        }

        return Ok(envelope.Data.Items.Select(permission => new
        {
            id = permission.Id,
            code = permission.Code,
            name = permission.Name
        }));
    }

    [HttpGet("{id:guid}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.GetMenuPermissionsAsync(
            id,
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(new { selected = Array.Empty<Guid>() });
        }

        return Ok(new { selected = envelope.Data.Items.Select(permission => permission.Id).ToArray() });
    }

    [HttpPut("{id:guid}/permissions")]
    public async Task<IActionResult> SetPermissions(
        Guid id,
        [FromBody] SetMenuPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.SetMenuPermissionsAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("seed-defaults")]
    public async Task<IActionResult> SeedDefaults(CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.SeedDefaultsAsync(cancellationToken);
        return envelope.ToJsonResult();
    }
}

