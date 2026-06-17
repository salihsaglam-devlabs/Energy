using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Application.Core.Localization.Commands.DeleteLocalizationEntry;
using Energy.Application.Core.Localization.Commands.UpsertLocalizationEntry;
using Energy.Application.Core.Localization.Queries.GetLocalizationByKey;
using Energy.Application.Core.Localization.Queries.GetLocalizationEntries;

namespace Energy.Api.Controllers.Core;

/// <summary>Çok dilli metin kaynakları uç noktaları (Core).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/localization")]
public sealed class LocalizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocalizationController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetLocalizationEntriesQuery(), ct));

    [HttpGet("{key}")]
    public async Task<ActionResult<BaseResponse<LocalizationEntryResponse>>> GetByKey(string key, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLocalizationByKeyQuery(key), ct));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<LocalizationEntryResponse>>> Upsert(UpsertLocalizationEntryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpsertLocalizationEntryCommand(request), ct));

    [HttpDelete("{key}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(string key, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteLocalizationEntryCommand(key), ct));
}
