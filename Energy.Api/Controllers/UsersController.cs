using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Application.Identity.Users.Commands.CreateUser;
using Energy.Application.Identity.Users.Commands.DeleteUser;
using Energy.Application.Identity.Users.Commands.RemoveProfileImage;
using Energy.Application.Identity.Users.Commands.SeedAdmin;
using Energy.Application.Identity.Users.Commands.SetUserRoles;
using Energy.Application.Identity.Users.Commands.UpdateProfileImage;
using Energy.Application.Identity.Users.Commands.UpdateUser;
using Energy.Application.Identity.Users.Commands.UpdateUserPassword;
using Energy.Application.Identity.Users.Queries.GetAdminPermissionHealth;
using Energy.Application.Identity.Users.Queries.GetProfileImage;
using Energy.Application.Identity.Users.Queries.GetUserById;
using Energy.Application.Identity.Users.Queries.GetUsers;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/users")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IUserService _userService;

    public UsersController(ISender sender, IUserService userService)
    {
        _sender = sender;
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Policy = UserPermissions.GetUsers)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = UserPermissions.GetUser)]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Returns the full profile (including roles) for the currently authenticated user.
    /// Available to any signed-in caller — no extra permission is required because the
    /// data returned belongs to the caller themselves.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = await ResolveCurrentUserIdAsync(cancellationToken);
        if (userId is null)
        {
            return Unauthorized();
        }

        var response = await _sender.Send(new GetUserByIdQuery(userId.Value), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Streams the raw profile image bytes. Returns 404 when the user has no image set.
    /// The response is NOT wrapped in <c>BaseResponse</c> so it can be rendered directly
    /// by an &lt;img&gt; tag.
    /// </summary>
    [HttpGet("{id:guid}/profile-image")]
    [Authorize]
    public async Task<IActionResult> GetProfileImage(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _sender.Send(new GetProfileImageQuery(id), cancellationToken);
        if (envelope.Data is null)
        {
            return NotFound();
        }

        // Disable caching so freshly uploaded avatars show up immediately.
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        return File(envelope.Data.Content, envelope.Data.ContentType);
    }

    /// <summary>
    /// Streams the profile image of the authenticated caller. Resolves the
    /// user id from the JWT subject claim so the client never has to pass it.
    /// </summary>
    [HttpGet("me/profile-image")]
    [Authorize]
    public async Task<IActionResult> GetCurrentProfileImage(CancellationToken cancellationToken)
    {
        var userId = await ResolveCurrentUserIdAsync(cancellationToken);
        if (userId is null)
        {
            return Unauthorized();
        }

        var envelope = await _sender.Send(new GetProfileImageQuery(userId.Value), cancellationToken);
        if (envelope.Data is null)
        {
            return NotFound();
        }

        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        return File(envelope.Data.Content, envelope.Data.ContentType);
    }

    /// <summary>
    /// Uploads a new profile image for the user. The caller must own the account or
    /// hold <see cref="UserPermissions.UpdateProfile"/>.
    /// </summary>
    [HttpPut("{id:guid}/profile-image")]
    [Authorize]
    [RequestSizeLimit(5_000_000)]
    public async Task<IActionResult> UpdateProfileImage(Guid id, [FromBody] UpdateProfileImageRequest request, CancellationToken cancellationToken)
    {
        if (!CanManageProfile(id))
        {
            return Forbid();
        }

        var response = await _sender.Send(
            new UpdateProfileImageCommand(id, request.Content, request.ContentType),
            cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Replaces the authenticated caller's profile image. Resolves the user
    /// id from the JWT so the call never depends on a (potentially stale)
    /// client-side id and cannot leak across accounts.
    /// </summary>
    [HttpPut("me/profile-image")]
    [Authorize]
    [RequestSizeLimit(5_000_000)]
    public async Task<IActionResult> UpdateCurrentProfileImage([FromBody] UpdateProfileImageRequest request, CancellationToken cancellationToken)
    {
        var userId = await ResolveCurrentUserIdAsync(cancellationToken);
        if (userId is null)
        {
            return Unauthorized();
        }

        var response = await _sender.Send(
            new UpdateProfileImageCommand(userId.Value, request.Content, request.ContentType),
            cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}/profile-image")]
    [Authorize]
    public async Task<IActionResult> RemoveProfileImage(Guid id, CancellationToken cancellationToken)
    {
        if (!CanManageProfile(id))
        {
            return Forbid();
        }

        var response = await _sender.Send(new RemoveProfileImageCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("me/profile-image")]
    [Authorize]
    public async Task<IActionResult> RemoveCurrentProfileImage(CancellationToken cancellationToken)
    {
        var userId = await ResolveCurrentUserIdAsync(cancellationToken);
        if (userId is null)
        {
            return Unauthorized();
        }

        var response = await _sender.Send(new RemoveProfileImageCommand(userId.Value), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = UserPermissions.CreateUser)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateUserCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = UserPermissions.UpdateUser)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateUserCommand(id, request), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/roles")]
    [Authorize(Policy = UserPermissions.SetRoles)]
    public async Task<IActionResult> SetRoles(Guid id, [FromBody] SetUserRolesRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SetUserRolesCommand(id, request.RoleIds), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/password")]
    [Authorize(Policy = UserPermissions.UpdatePassword)]
    public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdateUserPasswordRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateUserPasswordCommand(id, request.NewPassword), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = UserPermissions.DeleteUser)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteUserCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("admin-permissions/health")]
    [Authorize(Policy = UserPermissions.GetAdminPermissionHealth)]
    public async Task<IActionResult> GetAdminPermissionHealth(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetAdminPermissionHealthQuery(), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Seeds the default Admin role and Admin user, plus all default permissions linked to Admin role. Idempotent.
    /// Anonymous so the system can be bootstrapped before any token exists.
    /// </summary>
    [HttpPost("seed-admin")]
    [AllowAnonymous]
    public async Task<IActionResult> SeedAdmin(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SeedAdminCommand(), cancellationToken);
        return Ok(response);
    }

    private Guid? GetCurrentUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : null;
    }

    /// <summary>
    /// Resolves the database id of the currently authenticated user. Tries
    /// the JWT subject id first and falls back to the email / user name
    /// claims so the call still works after a database reseed gives the user
    /// a brand new row id (the still-valid token would otherwise point at a
    /// vanished record).
    /// </summary>
    private Task<Guid?> ResolveCurrentUserIdAsync(CancellationToken cancellationToken)
    {
        return _userService.ResolveCurrentUserIdAsync(
            GetCurrentUserId(),
            User.FindFirstValue(ClaimTypes.Email),
            User.FindFirstValue(ClaimTypes.Name),
            cancellationToken);
    }

    private bool CanManageProfile(Guid targetUserId)
    {
        var currentId = GetCurrentUserId();
        if (currentId == targetUserId)
        {
            return true;
        }

        // Users with the explicit administrative permission can manage any account.
        return User.HasClaim("permission", UserPermissions.UpdateProfile)
            || User.HasClaim("permission", UserPermissions.UpdateUser);
    }
}
