using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Core.UnitConversion.Commands.CreateUnitConversion;
using Energy.Application.Core.UnitConversion.Commands.DeleteUnitConversion;
using Energy.Application.Core.UnitConversion.Commands.UpdateUnitConversion;
using Energy.Application.Core.UnitConversion.Queries.GetUnitConversionById;
using Energy.Application.Core.UnitConversion.Queries.GetUnitConversionList;
using Energy.Application.Core.UnitConversion.Queries.GetUnitConversionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// UnitConversion uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/unit-conversions")]
public sealed class UnitConversionController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitConversionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<UnitConversionListResponse>>>> GetList([FromQuery] GetUnitConversionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUnitConversionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<UnitConversionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUnitConversionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUnitConversionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateUnitConversionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateUnitConversionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateUnitConversionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateUnitConversionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteUnitConversionCommand(id), ct));
}
