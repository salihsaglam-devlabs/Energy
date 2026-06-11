using Asp.Versioning;
using Energy.Application.Identity.Roles.Commands.CreateRole;
using Energy.Application.Identity.Roles.Commands.DeleteRole;
using Energy.Application.Identity.Roles.Commands.SetRoleMenus;
using Energy.Application.Identity.Roles.Commands.SetRolePermissions;
using Energy.Application.Identity.Roles.Commands.UpdateRole;
using Energy.Application.Identity.Roles.Queries.GetRoleById;
using Energy.Application.Identity.Roles.Queries.GetRoleMenus;
using Energy.Application.Identity.Roles.Queries.GetRolePermissions;
using Energy.Application.Identity.Roles.Queries.GetRoles;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/roles")]
[Authorize]
public sealed class RolesController : ControllerBase
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = RolePermissions.GetRoles)]
    public async Task<IActionResult> GetRoles([FromQuery] GetRolesQuery query, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = RolePermissions.GetRole)]
    public async Task<IActionResult> GetRole(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetRoleByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = RolePermissions.CreateRole)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateRoleCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = RolePermissions.UpdateRole)]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateRoleCommand(id, request), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = RolePermissions.DeleteRole)]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteRoleCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Policy = RolePermissions.GetRolePermissions)]
    public async Task<IActionResult> GetRolePermissions(
        Guid id,
        [FromQuery] GetRolePermissionsQuery query,
        CancellationToken cancellationToken)
    {
        var effectiveQuery = new GetRolePermissionsQuery(id)
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
    [Authorize(Policy = RolePermissions.SetRolePermissions)]
    public async Task<IActionResult> SetRolePermissions(Guid id, [FromBody] SetRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SetRolePermissionsCommand(id, request.PermissionIds), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}/menus")]
    [Authorize(Policy = RolePermissions.GetRoleMenus)]
    public async Task<IActionResult> GetRoleMenus(
        Guid id,
        [FromQuery] GetRoleMenusQuery query,
        CancellationToken cancellationToken)
    {
        var effectiveQuery = new GetRoleMenusQuery(id)
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

    [HttpPut("{id:guid}/menus")]
    [Authorize(Policy = RolePermissions.SetRoleMenus)]
    public async Task<IActionResult> SetRoleMenus(Guid id, [FromBody] SetRoleMenusRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SetRoleMenusCommand(id, request.MenuIds), cancellationToken);
        return Ok(response);
    }
}
