using Asp.Versioning;
using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _users;
    public UsersController(IUserService users) { _users = users; }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<UserSummaryResponse>>>> GetAll(
        [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(BaseResponse<PaginatedResponse<UserSummaryResponse>>.Success(await _users.GetAllAsync(request, ct)));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> GetById(Guid id, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, id);
        return Ok(BaseResponse<UserDetailResponse>.Success(user));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> Create(CreateUserRequest request, CancellationToken ct)
        => Ok(BaseResponse<UserDetailResponse>.Success(await _users.CreateAsync(request, ct)));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> Update(Guid id, UpdateUserRequest request, CancellationToken ct)
        => Ok(BaseResponse<UserDetailResponse>.Success(await _users.UpdateAsync(id, request, ct)));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _users.DeleteAsync(id, ct)));

    [HttpPut("{id:guid}/password")]
    public async Task<ActionResult<BaseResponse<bool>>> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _users.ChangePasswordAsync(id, request, ct)));

    [HttpGet("{id:guid}/access")]
    public async Task<ActionResult<BaseResponse<UserAccessResponse>>> GetAccess(Guid id, CancellationToken ct)
    {
        var access = await _users.GetAccessAsync(id, ct)
                     ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, id);
        return Ok(BaseResponse<UserAccessResponse>.Success(access));
    }

    [HttpPut("{id:guid}/access")]
    public async Task<ActionResult<BaseResponse<UserAccessResponse>>> SetAccess(Guid id, SetUserAccessRequest request, CancellationToken ct)
        => Ok(BaseResponse<UserAccessResponse>.Success(await _users.SetAccessAsync(id, request, ct)));

    [HttpGet("{id:guid}/profile-image")]
    public async Task<IActionResult> GetProfileImage(Guid id, CancellationToken ct)
    {
        var image = await _users.GetProfileImageAsync(id, ct);
        return image is null ? NotFound() : File(image.Content, image.ContentType);
    }

    [HttpPut("{id:guid}/profile-image")]
    public async Task<ActionResult<BaseResponse<bool>>> SetProfileImage(Guid id, SetProfileImageRequest request, CancellationToken ct)
    {
        var content = Convert.FromBase64String(request.ContentBase64);
        var ok = await _users.SetProfileImageAsync(id, content, request.ContentType, ct);
        if (!ok) throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, id);
        return Ok(BaseResponse<bool>.Success(true));
    }

    [HttpDelete("{id:guid}/profile-image")]
    public async Task<ActionResult<BaseResponse<bool>>> RemoveProfileImage(Guid id, CancellationToken ct)
    {
        var ok = await _users.RemoveProfileImageAsync(id, ct);
        if (!ok) throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, id);
        return Ok(BaseResponse<bool>.Success(true));
    }
}
