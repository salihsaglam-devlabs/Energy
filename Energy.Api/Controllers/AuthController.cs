using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IUserService _users;
    private readonly IStringLocalizer<SharedResource> _localizer;
    public AuthController(IUserService users, IStringLocalizer<SharedResource> localizer)
    {
        _users = users;
        _localizer = localizer;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponse<AuthTokenResponse>>> Login(LoginRequest request, CancellationToken ct)
    {
        var token = await _users.LoginAsync(request, ct);
        return token is null
            ? Unauthorized(BaseResponse<AuthTokenResponse>.Failure(_localizer[LocalizationKeys.Messages.InvalidCredentials].Value))
            : Ok(BaseResponse<AuthTokenResponse>.Success(token));
    }
}
