using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.IAM.User.Commands.ChangeUserPassword;
using Energy.Application.IAM.User.Commands.CreateUser;
using Energy.Application.IAM.User.Commands.DeleteUser;
using Energy.Application.IAM.User.Commands.RemoveUserProfileImage;
using Energy.Application.IAM.User.Commands.SetUserAccess;
using Energy.Application.IAM.User.Commands.SetUserProfileImage;
using Energy.Application.IAM.User.Commands.UpdateUser;
using Energy.Application.IAM.User.Queries.GetMyProfile;
using Energy.Application.IAM.User.Queries.GetUserAccess;
using Energy.Application.IAM.User.Queries.GetUserById;
using Energy.Application.IAM.User.Queries.GetUserList;
using Energy.Application.IAM.User.Queries.GetUserProfileImage;

namespace Energy.Api.Controllers.IAM;

/// <summary>Kullanıcı yönetimi uç noktaları (IAM).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<UserSummaryResponse>>>> GetAll([FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserListQuery(request), ct));

    [HttpGet("me")]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> GetMine(CancellationToken ct)
        => Ok(await _mediator.Send(new GetMyProfileQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserByIdQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> Create(CreateUserRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateUserCommand(request), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<UserDetailResponse>>> Update(Guid id, UpdateUserRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateUserCommand(id, request), ct));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteUserCommand(id), ct));

    [HttpPut("{id:guid}/password")]
    public async Task<ActionResult<BaseResponse<bool>>> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ChangeUserPasswordCommand(id, request), ct));

    [HttpGet("{id:guid}/access")]
    public async Task<ActionResult<BaseResponse<UserAccessResponse>>> GetAccess(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserAccessQuery(id), ct));

    [HttpPut("{id:guid}/access")]
    public async Task<ActionResult<BaseResponse<UserAccessResponse>>> SetAccess(Guid id, SetUserAccessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new SetUserAccessCommand(id, request), ct));

    [HttpGet("{id:guid}/profile-image")]
    public async Task<IActionResult> GetProfileImage(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserProfileImageQuery(id), ct);
        return result is null ? NotFound() : File(result.Content, result.ContentType);
    }

    [HttpPut("{id:guid}/profile-image")]
    public async Task<ActionResult<BaseResponse<bool>>> SetProfileImage(Guid id, SetProfileImageRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new SetUserProfileImageCommand(id, request), ct));

    [HttpDelete("{id:guid}/profile-image")]
    public async Task<ActionResult<BaseResponse<bool>>> RemoveProfileImage(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new RemoveUserProfileImageCommand(id), ct));
}
