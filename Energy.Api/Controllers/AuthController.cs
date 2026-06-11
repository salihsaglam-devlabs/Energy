using Asp.Versioning;
using Energy.Application.Identity.Auth.Commands.Login;
using Energy.Application.Identity.Auth.Queries.ValidateCredentials;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/auth")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Issues a JWT bearer access token for the supplied credentials.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new LoginCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPost("validate-credentials")]
    public async Task<IActionResult> ValidateCredentials(
        [FromBody] ValidateCredentialsRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new ValidateCredentialsQuery(request), cancellationToken);
        return Ok(response);
    }
}
