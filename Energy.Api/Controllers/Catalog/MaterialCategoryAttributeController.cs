using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.CreateMaterialCategoryAttribute;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.DeleteMaterialCategoryAttribute;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.UpdateMaterialCategoryAttribute;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeById;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeList;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// MaterialCategoryAttribute uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/material-category-attributes")]
public sealed class MaterialCategoryAttributeController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialCategoryAttributeController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>>> GetList([FromQuery] GetMaterialCategoryAttributeListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialCategoryAttributeListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MaterialCategoryAttributeDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialCategoryAttributeByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialCategoryAttributeLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMaterialCategoryAttributeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMaterialCategoryAttributeCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMaterialCategoryAttributeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMaterialCategoryAttributeCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMaterialCategoryAttributeCommand(id), ct));
}
