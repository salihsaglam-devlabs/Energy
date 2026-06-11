using Energy.Localization;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
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
[Route("roles")]
[Route("system/roles")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class RolesController : Controller
{
    private readonly IRoleApiClient _roleApiClient;
    private readonly IPermissionApiClient _permissionApiClient;
    private readonly IMenuApiClient _menuApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RolesController(
        IRoleApiClient roleApiClient,
        IPermissionApiClient permissionApiClient,
        IMenuApiClient menuApiClient,
        IStringLocalizer<SharedResource> localizer)
    {
        _roleApiClient = roleApiClient;
        _permissionApiClient = permissionApiClient;
        _menuApiClient = menuApiClient;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.RolesScreen.Title);
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] GridLoadOptions options, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.GetRolesAsync(options.ToPaginatedRequest(), cancellationToken);
        return envelope.ToGridResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.GetRoleAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.CreateRoleAsync(request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.UpdateRoleAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.DeleteRoleAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    // ---- Role permissions ---------------------------------------------------

    [HttpGet("{id:guid}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.GetRolePermissionsAsync(
            id,
            new PaginatedRequest { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(new { selected = Array.Empty<Guid>() });
        }

        return Ok(new { selected = envelope.Data.Items.Select(p => p.Id).ToArray() });
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

        return Ok(envelope.Data.Items.Select(p => new { id = p.Id, code = p.Code, name = p.Name }));
    }

    [HttpPut("{id:guid}/permissions")]
    public async Task<IActionResult> SetPermissions(
        Guid id,
        [FromBody] SetRolePermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.SetRolePermissionsAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    // ---- Role menus ---------------------------------------------------------

    [HttpGet("{id:guid}/menus")]
    public async Task<IActionResult> GetMenus(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.GetRoleMenusAsync(
            id,
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(new { selected = Array.Empty<Guid>() });
        }

        return Ok(new { selected = envelope.Data.Items.Select(m => m.Id).ToArray() });
    }

    [HttpGet("menus-tree")]
    public async Task<IActionResult> MenusTree(CancellationToken cancellationToken)
    {
        var envelope = await _menuApiClient.GetMenusAsync(
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(Array.Empty<object>());
        }

        // Flat list with parent links, suitable for dxTreeView dataStructure: 'plain'.
        return Ok(envelope.Data.Items.Select(m => new
        {
            id = m.Id,
            parentId = m.ParentId,
            text = string.IsNullOrEmpty(m.Name) ? m.NameKey : m.Name,
            order = m.Order
        }));
    }

    [HttpPut("{id:guid}/menus")]
    public async Task<IActionResult> SetMenus(
        Guid id,
        [FromBody] SetRoleMenusRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.SetRoleMenusAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }
}

