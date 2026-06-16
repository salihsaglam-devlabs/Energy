using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockTransaction.Commands.CreateStockTransaction;
using Energy.Application.Modules.Inventory.StockTransaction.Commands.DeleteStockTransaction;
using Energy.Application.Modules.Inventory.StockTransaction.Commands.UpdateStockTransaction;
using Energy.Application.Modules.Inventory.StockTransaction.Queries.GetStockTransactionById;
using Energy.Application.Modules.Inventory.StockTransaction.Queries.GetStockTransactionList;
using Energy.Application.Modules.Inventory.StockTransaction.Queries.GetStockTransactionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockTransaction uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-transactions")]
public sealed class StockTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockTransactionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockTransactionListResponse>>>> GetList([FromQuery] GetStockTransactionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockTransactionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockTransactionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockTransactionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockTransactionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockTransactionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockTransactionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockTransactionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockTransactionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockTransactionCommand(id), ct));
}
