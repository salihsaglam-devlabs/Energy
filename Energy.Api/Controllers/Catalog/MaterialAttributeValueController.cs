using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Catalog.MaterialAttributeValue.Commands.CreateMaterialAttributeValue;
using Energy.Application.Catalog.MaterialAttributeValue.Commands.DeleteMaterialAttributeValue;
using Energy.Application.Catalog.MaterialAttributeValue.Commands.UpdateMaterialAttributeValue;
using Energy.Application.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueById;
using Energy.Application.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueList;
using Energy.Application.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// MaterialAttributeValue uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/material-attribute-values")]
public sealed class MaterialAttributeValueController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialAttributeValueController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>>> GetList([FromQuery] GetMaterialAttributeValueListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialAttributeValueListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MaterialAttributeValueDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialAttributeValueByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialAttributeValueLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMaterialAttributeValueRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMaterialAttributeValueCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMaterialAttributeValueRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMaterialAttributeValueCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMaterialAttributeValueCommand(id), ct));
}
