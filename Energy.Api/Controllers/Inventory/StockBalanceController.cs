using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockBalance.Commands.CreateStockBalance;
using Energy.Application.Modules.Inventory.StockBalance.Commands.DeleteStockBalance;
using Energy.Application.Modules.Inventory.StockBalance.Commands.UpdateStockBalance;
using Energy.Application.Modules.Inventory.StockBalance.Queries.GetStockBalanceById;
using Energy.Application.Modules.Inventory.StockBalance.Queries.GetStockBalanceList;
using Energy.Application.Modules.Inventory.StockBalance.Queries.GetStockBalanceLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockBalance uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-balances")]
public sealed class StockBalanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockBalanceController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockBalanceListResponse>>>> GetList([FromQuery] GetStockBalanceListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockBalanceListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockBalanceDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockBalanceByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockBalanceLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockBalanceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockBalanceCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockBalanceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockBalanceCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockBalanceCommand(id), ct));
}
