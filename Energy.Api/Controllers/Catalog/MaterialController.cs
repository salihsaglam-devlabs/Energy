using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Catalog.Material.Commands.CreateMaterial;
using Energy.Application.Modules.Catalog.Material.Commands.DeleteMaterial;
using Energy.Application.Modules.Catalog.Material.Commands.UpdateMaterial;
using Energy.Application.Modules.Catalog.Material.Queries.GetMaterialById;
using Energy.Application.Modules.Catalog.Material.Queries.GetMaterialList;
using Energy.Application.Modules.Catalog.Material.Queries.GetMaterialLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Requests;
using Energy.Shared.Models.V1.Catalog.Material.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// Material uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/materials")]
public sealed class MaterialController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MaterialListResponse>>>> GetList([FromQuery] GetMaterialListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MaterialDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MaterialLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMaterialLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMaterialRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMaterialCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMaterialRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMaterialCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMaterialCommand(id), ct));
}
