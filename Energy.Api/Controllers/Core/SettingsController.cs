using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using Energy.Application.Modules.Core.UserSettings.Commands.UpdateMySettings;
using Energy.Application.Modules.Core.UserSettings.Queries.GetMySettings;

namespace Energy.Api.Controllers.Core;

/// <summary>Self-servis kullanıcı ayarları uç noktaları (Core).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/settings")]
public sealed class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("me")]
    public async Task<ActionResult<BaseResponse<UserSettingsResponse>>> GetMine(CancellationToken ct)
        => Ok(await _mediator.Send(new GetMySettingsQuery(), ct));

    [HttpPut("me")]
    public async Task<ActionResult<BaseResponse<UserSettingsResponse>>> UpdateMine(UpdateUserSettingsRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMySettingsCommand(request), ct));
}
