using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Commands.CreateWarehouseTransferLine;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Commands.DeleteWarehouseTransferLine;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Commands.UpdateWarehouseTransferLine;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineById;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineList;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// WarehouseTransferLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/warehouse-transfer-lines")]
public sealed class WarehouseTransferLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseTransferLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WarehouseTransferLineListResponse>>>> GetList([FromQuery] GetWarehouseTransferLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWarehouseTransferLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WarehouseTransferLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWarehouseTransferLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWarehouseTransferLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWarehouseTransferLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWarehouseTransferLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWarehouseTransferLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWarehouseTransferLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWarehouseTransferLineCommand(id), ct));
}
