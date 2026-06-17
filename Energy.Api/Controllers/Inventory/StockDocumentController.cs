using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Inventory.StockDocument.Commands.CreateStockDocument;
using Energy.Application.Inventory.StockDocument.Commands.DeleteStockDocument;
using Energy.Application.Inventory.StockDocument.Commands.UpdateStockDocument;
using Energy.Application.Inventory.StockDocument.Queries.GetStockDocumentById;
using Energy.Application.Inventory.StockDocument.Queries.GetStockDocumentList;
using Energy.Application.Inventory.StockDocument.Queries.GetStockDocumentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockDocument uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-documents")]
public sealed class StockDocumentController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockDocumentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockDocumentListResponse>>>> GetList([FromQuery] GetStockDocumentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockDocumentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockDocumentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockDocumentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockDocumentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockDocumentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockDocumentCommand(id), ct));
}
