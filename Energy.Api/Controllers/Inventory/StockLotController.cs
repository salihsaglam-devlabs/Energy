using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Inventory.StockLot.Commands.CreateStockLot;
using Energy.Application.Inventory.StockLot.Commands.DeleteStockLot;
using Energy.Application.Inventory.StockLot.Commands.UpdateStockLot;
using Energy.Application.Inventory.StockLot.Queries.GetStockLotById;
using Energy.Application.Inventory.StockLot.Queries.GetStockLotList;
using Energy.Application.Inventory.StockLot.Queries.GetStockLotLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockLot uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-lots")]
public sealed class StockLotController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockLotController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockLotListResponse>>>> GetList([FromQuery] GetStockLotListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockLotListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockLotDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockLotByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockLotLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockLotLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockLotRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockLotCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockLotRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockLotCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockLotCommand(id), ct));
}
