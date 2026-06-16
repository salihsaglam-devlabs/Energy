using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Catalog.MaterialAttributeDefinition.Commands.CreateMaterialAttributeDefinition;
using Energy.Application.Catalog.MaterialAttributeDefinition.Commands.DeleteMaterialAttributeDefinition;
using Energy.Application.Catalog.MaterialAttributeDefinition.Commands.UpdateMaterialAttributeDefinition;
using Energy.Application.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionById;
using Energy.Application.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionList;
using Energy.Application.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// MaterialAttributeDefinition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/material-attribute-definitions")]
public sealed class MaterialAttributeDefinitionController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialAttributeDefinitionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>>> GetList([FromQuery] GetMaterialAttributeDefinitionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialAttributeDefinitionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MaterialAttributeDefinitionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialAttributeDefinitionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialAttributeDefinitionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMaterialAttributeDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMaterialAttributeDefinitionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMaterialAttributeDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMaterialAttributeDefinitionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMaterialAttributeDefinitionCommand(id), ct));
}
