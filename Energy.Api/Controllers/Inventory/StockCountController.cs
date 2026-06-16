using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockCount.Commands.CreateStockCount;
using Energy.Application.Modules.Inventory.StockCount.Commands.DeleteStockCount;
using Energy.Application.Modules.Inventory.StockCount.Commands.UpdateStockCount;
using Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountById;
using Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountList;
using Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockCount uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-counts")]
public sealed class StockCountController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockCountController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockCountListResponse>>>> GetList([FromQuery] GetStockCountListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockCountListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockCountDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockCountByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockCountLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockCountLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockCountRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockCountCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockCountRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockCountCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockCountCommand(id), ct));
}
