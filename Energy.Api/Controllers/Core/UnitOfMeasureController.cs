using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Core.UnitOfMeasure.Commands.CreateUnitOfMeasure;
using Energy.Application.Modules.Core.UnitOfMeasure.Commands.DeleteUnitOfMeasure;
using Energy.Application.Modules.Core.UnitOfMeasure.Commands.UpdateUnitOfMeasure;
using Energy.Application.Modules.Core.UnitOfMeasure.Queries.GetUnitOfMeasureById;
using Energy.Application.Modules.Core.UnitOfMeasure.Queries.GetUnitOfMeasureList;
using Energy.Application.Modules.Core.UnitOfMeasure.Queries.GetUnitOfMeasureLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// UnitOfMeasure uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/units-of-measure")]
public sealed class UnitOfMeasureController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitOfMeasureController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>>> GetList([FromQuery] GetUnitOfMeasureListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUnitOfMeasureListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<UnitOfMeasureDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUnitOfMeasureByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUnitOfMeasureLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateUnitOfMeasureRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateUnitOfMeasureCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateUnitOfMeasureRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateUnitOfMeasureCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteUnitOfMeasureCommand(id), ct));
}
