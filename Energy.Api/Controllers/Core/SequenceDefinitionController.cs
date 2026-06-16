using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Core.SequenceDefinition.Commands.CreateSequenceDefinition;
using Energy.Application.Modules.Core.SequenceDefinition.Commands.DeleteSequenceDefinition;
using Energy.Application.Modules.Core.SequenceDefinition.Commands.UpdateSequenceDefinition;
using Energy.Application.Modules.Core.SequenceDefinition.Queries.GetSequenceDefinitionById;
using Energy.Application.Modules.Core.SequenceDefinition.Queries.GetSequenceDefinitionList;
using Energy.Application.Modules.Core.SequenceDefinition.Queries.GetSequenceDefinitionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// SequenceDefinition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/sequence-definitions")]
public sealed class SequenceDefinitionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SequenceDefinitionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>>> GetList([FromQuery] GetSequenceDefinitionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSequenceDefinitionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SequenceDefinitionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSequenceDefinitionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSequenceDefinitionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSequenceDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateSequenceDefinitionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSequenceDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSequenceDefinitionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteSequenceDefinitionCommand(id), ct));
}
