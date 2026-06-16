using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.Seeding.Commands.SeedAll;
using Energy.Application.Core.Seeding.Commands.SeedLocalization;
using Energy.Application.Core.Seeding.Commands.SeedLocalizationFromResx;

namespace Energy.Api.Controllers.Core;

/// <summary>İdempotent veri tohumlama uç noktaları (Core).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/seed")]
public sealed class SeedController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeedController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> SeedAll(CancellationToken ct)
        => Ok(await _mediator.Send(new SeedAllCommand(), ct));

    [HttpPost("localization")]
    public async Task<ActionResult<BaseResponse<SeedResultResponse>>> SeedLocalization(CancellationToken ct)
        => Ok(await _mediator.Send(new SeedLocalizationCommand(), ct));

    [HttpPost("localization/resx")]
    public async Task<ActionResult<BaseResponse<SeedResultResponse>>> SeedLocalizationFromResx(CancellationToken ct)
        => Ok(await _mediator.Send(new SeedLocalizationFromResxCommand(), ct));
}
