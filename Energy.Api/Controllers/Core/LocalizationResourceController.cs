using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Core.LocalizationResource.Commands.CreateLocalizationResource;
using Energy.Application.Core.LocalizationResource.Commands.DeleteLocalizationResource;
using Energy.Application.Core.LocalizationResource.Commands.UpdateLocalizationResource;
using Energy.Application.Core.LocalizationResource.Queries.GetLocalizationResourceById;
using Energy.Application.Core.LocalizationResource.Queries.GetLocalizationResourceList;
using Energy.Application.Core.LocalizationResource.Queries.GetLocalizationResourceLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// LocalizationResource uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/localization-resources")]
public sealed class LocalizationResourceController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocalizationResourceController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>>> GetList([FromQuery] GetLocalizationResourceListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLocalizationResourceListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<LocalizationResourceDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLocalizationResourceByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLocalizationResourceLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateLocalizationResourceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateLocalizationResourceCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateLocalizationResourceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateLocalizationResourceCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteLocalizationResourceCommand(id), ct));
}
