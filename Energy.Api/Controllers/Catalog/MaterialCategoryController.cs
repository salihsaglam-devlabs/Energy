using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Catalog.MaterialCategory.Commands.CreateMaterialCategory;
using Energy.Application.Catalog.MaterialCategory.Commands.DeleteMaterialCategory;
using Energy.Application.Catalog.MaterialCategory.Commands.UpdateMaterialCategory;
using Energy.Application.Catalog.MaterialCategory.Queries.GetMaterialCategoryById;
using Energy.Application.Catalog.MaterialCategory.Queries.GetMaterialCategoryList;
using Energy.Application.Catalog.MaterialCategory.Queries.GetMaterialCategoryLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// MaterialCategory uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/material-categories")]
public sealed class MaterialCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialCategoryController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>>> GetList([FromQuery] GetMaterialCategoryListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialCategoryListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MaterialCategoryDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialCategoryByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialCategoryLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMaterialCategoryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMaterialCategoryCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMaterialCategoryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMaterialCategoryCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMaterialCategoryCommand(id), ct));
}
