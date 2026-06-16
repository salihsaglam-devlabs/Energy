using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockDocumentType.Commands.CreateStockDocumentType;
using Energy.Application.Modules.Inventory.StockDocumentType.Commands.DeleteStockDocumentType;
using Energy.Application.Modules.Inventory.StockDocumentType.Commands.UpdateStockDocumentType;
using Energy.Application.Modules.Inventory.StockDocumentType.Queries.GetStockDocumentTypeById;
using Energy.Application.Modules.Inventory.StockDocumentType.Queries.GetStockDocumentTypeList;
using Energy.Application.Modules.Inventory.StockDocumentType.Queries.GetStockDocumentTypeLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockDocumentType uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-document-types")]
public sealed class StockDocumentTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockDocumentTypeController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockDocumentTypeListResponse>>>> GetList([FromQuery] GetStockDocumentTypeListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentTypeListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockDocumentTypeDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentTypeByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockDocumentTypeLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockDocumentTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockDocumentTypeCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockDocumentTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockDocumentTypeCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockDocumentTypeCommand(id), ct));
}
