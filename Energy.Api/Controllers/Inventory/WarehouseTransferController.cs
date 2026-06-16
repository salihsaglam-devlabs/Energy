using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Commands.CreateWarehouseTransfer;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Commands.DeleteWarehouseTransfer;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Commands.UpdateWarehouseTransfer;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferById;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferList;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// WarehouseTransfer uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/warehouse-transfers")]
public sealed class WarehouseTransferController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseTransferController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>>> GetList([FromQuery] GetWarehouseTransferListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWarehouseTransferListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WarehouseTransferDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWarehouseTransferByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWarehouseTransferLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWarehouseTransferRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWarehouseTransferCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWarehouseTransferRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWarehouseTransferCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWarehouseTransferCommand(id), ct));
}
