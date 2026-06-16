using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockDocumentLine.Commands.CreateStockDocumentLine;
using Energy.Application.Modules.Inventory.StockDocumentLine.Commands.DeleteStockDocumentLine;
using Energy.Application.Modules.Inventory.StockDocumentLine.Commands.UpdateStockDocumentLine;
using Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineById;
using Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineList;
using Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockDocumentLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-document-lines")]
public sealed class StockDocumentLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockDocumentLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>>> GetList([FromQuery] GetStockDocumentLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockDocumentLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockDocumentLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockDocumentLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockDocumentLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockDocumentLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockDocumentLineCommand(id), ct));
}
