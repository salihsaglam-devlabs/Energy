using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Catalog.Brand.Commands.CreateBrand;
using Energy.Application.Modules.Catalog.Brand.Commands.DeleteBrand;
using Energy.Application.Modules.Catalog.Brand.Commands.UpdateBrand;
using Energy.Application.Modules.Catalog.Brand.Queries.GetBrandById;
using Energy.Application.Modules.Catalog.Brand.Queries.GetBrandList;
using Energy.Application.Modules.Catalog.Brand.Queries.GetBrandLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;

namespace Energy.Api.Controllers.Catalog;

/// <summary>
/// Brand uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog/brands")]
public sealed class BrandController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BrandListResponse>>>> GetList([FromQuery] GetBrandListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBrandListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BrandDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBrandByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BrandLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBrandLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBrandRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBrandCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBrandRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBrandCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBrandCommand(id), ct));
}
