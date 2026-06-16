using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Modules.IAM.Auth.Commands.Login;

namespace Energy.Api.Controllers.IAM;

/// <summary>Kimlik doğrulama uç noktaları (login).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponse<AuthTokenResponse>>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(request), ct);
        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
}
