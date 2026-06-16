using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockCountLine.Commands.CreateStockCountLine;
using Energy.Application.Modules.Inventory.StockCountLine.Commands.DeleteStockCountLine;
using Energy.Application.Modules.Inventory.StockCountLine.Commands.UpdateStockCountLine;
using Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineById;
using Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineList;
using Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockCountLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-count-lines")]
public sealed class StockCountLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockCountLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockCountLineListResponse>>>> GetList([FromQuery] GetStockCountLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockCountLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockCountLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockCountLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockCountLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockCountLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockCountLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockCountLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockCountLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockCountLineCommand(id), ct));
}
