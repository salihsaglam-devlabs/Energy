using Asp.Versioning;
using Energy.Application.Identity.Permissions.Commands.CreatePermission;
using Energy.Application.Identity.Permissions.Commands.DeletePermission;
using Energy.Application.Identity.Permissions.Commands.SeedDefaultPermissions;
using Energy.Application.Identity.Permissions.Commands.UpdatePermission;
using Energy.Application.Identity.Permissions.Queries.GetPermissionById;
using Energy.Application.Identity.Permissions.Queries.GetPermissions;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/permissions")]
[Authorize]
public sealed class PermissionsController : ControllerBase
{
    private readonly ISender _sender;

    public PermissionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = PermissionPermissions.GetPermissions)]
    public async Task<IActionResult> GetPermissions([FromQuery] GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionPermissions.GetPermission)]
    public async Task<IActionResult> GetPermission(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetPermissionByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = PermissionPermissions.CreatePermission)]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreatePermissionCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionPermissions.UpdatePermission)]
    public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdatePermissionCommand(id, request), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionPermissions.DeletePermission)]
    public async Task<IActionResult> DeletePermission(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeletePermissionCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost("seed-defaults")]
    [AllowAnonymous]
    public async Task<IActionResult> SeedDefaults(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SeedDefaultPermissionsCommand(), cancellationToken);
        return Ok(response);
    }
}
