using Asp.Versioning;
using Energy.Application.System.Menus.Commands.CreateMenu;
using Energy.Application.System.Menus.Commands.DeleteMenu;
using Energy.Application.System.Menus.Commands.SeedDefaultMenus;
using Energy.Application.System.Menus.Commands.SetMenuPermissions;
using Energy.Application.System.Menus.Commands.UpdateMenu;
using Energy.Application.System.Menus.Queries.GetMenuById;
using Energy.Application.System.Menus.Queries.GetMenuPermissions;
using Energy.Application.System.Menus.Queries.GetMenus;
using Energy.Application.System.Menus.Queries.GetMenuTree;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/menus")]
[Authorize]
public sealed class MenusController : ControllerBase
{
    private readonly ISender _sender;

    public MenusController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = MenuPermissions.GetMenus)]
    public async Task<IActionResult> GetMenus([FromQuery] GetMenusQuery query, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Returns the hierarchical menu tree (roots with nested children) with
    /// names resolved to the current request culture.
    /// </summary>
    [HttpGet("tree")]
    [Authorize(Policy = MenuPermissions.GetMenuTree)]
    public async Task<IActionResult> GetMenuTree(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetMenuTreeQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = MenuPermissions.GetMenu)]
    public async Task<IActionResult> GetMenu(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetMenuByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = MenuPermissions.CreateMenu)]
    public async Task<IActionResult> CreateMenu([FromBody] CreateMenuRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateMenuCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = MenuPermissions.UpdateMenu)]
    public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] UpdateMenuRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateMenuCommand(id, request), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = MenuPermissions.DeleteMenu)]
    public async Task<IActionResult> DeleteMenu(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteMenuCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Policy = MenuPermissions.GetMenuPermissions)]
    public async Task<IActionResult> GetMenuPermissions(
        Guid id,
        [FromQuery] GetMenuPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        var effectiveQuery = new GetMenuPermissionsQuery(id)
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy,
            IsDescending = query.IsDescending,
            Filters = query.Filters
        };

        var response = await _sender.Send(effectiveQuery, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = MenuPermissions.SetMenuPermissions)]
    public async Task<IActionResult> SetMenuPermissions(
        Guid id,
        [FromBody] SetMenuPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SetMenuPermissionsCommand(id, request.PermissionIds), cancellationToken);
        return Ok(response);
    }

    [HttpPost("seed-defaults")]
    [AllowAnonymous]
    public async Task<IActionResult> SeedDefaults(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SeedDefaultMenusCommand(), cancellationToken);
        return Ok(response);
    }
}
