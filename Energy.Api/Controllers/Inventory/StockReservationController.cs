using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockReservation.Commands.CreateStockReservation;
using Energy.Application.Modules.Inventory.StockReservation.Commands.DeleteStockReservation;
using Energy.Application.Modules.Inventory.StockReservation.Commands.UpdateStockReservation;
using Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationById;
using Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationList;
using Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockReservation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-reservations")]
public sealed class StockReservationController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockReservationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockReservationListResponse>>>> GetList([FromQuery] GetStockReservationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockReservationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockReservationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockReservationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockReservationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockReservationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockReservationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockReservationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockReservationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockReservationCommand(id), ct));
}
