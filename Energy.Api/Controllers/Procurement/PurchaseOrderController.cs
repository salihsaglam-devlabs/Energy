using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Procurement.PurchaseOrder.Commands.CreatePurchaseOrder;
using Energy.Application.Procurement.PurchaseOrder.Commands.DeletePurchaseOrder;
using Energy.Application.Procurement.PurchaseOrder.Commands.UpdatePurchaseOrder;
using Energy.Application.Procurement.PurchaseOrder.Queries.GetPurchaseOrderById;
using Energy.Application.Procurement.PurchaseOrder.Queries.GetPurchaseOrderList;
using Energy.Application.Procurement.PurchaseOrder.Queries.GetPurchaseOrderLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// PurchaseOrder uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/purchase-orders")]
public sealed class PurchaseOrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseOrderController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>>> GetList([FromQuery] GetPurchaseOrderListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PurchaseOrderDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePurchaseOrderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePurchaseOrderCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePurchaseOrderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePurchaseOrderCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePurchaseOrderCommand(id), ct));
}
