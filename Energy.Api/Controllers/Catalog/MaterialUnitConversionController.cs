using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.CreateMaterialUnitConversion;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.DeleteMaterialUnitConversion;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.UpdateMaterialUnitConversion;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionById;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionList;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// MaterialUnitConversion uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/material-unit-conversions")]
public sealed class MaterialUnitConversionController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialUnitConversionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>>> GetList([FromQuery] GetMaterialUnitConversionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialUnitConversionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MaterialUnitConversionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialUnitConversionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialUnitConversionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMaterialUnitConversionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMaterialUnitConversionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMaterialUnitConversionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMaterialUnitConversionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMaterialUnitConversionCommand(id), ct));
}
