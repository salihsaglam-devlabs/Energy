using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Procurement.PurchaseOrderLine.Commands.CreatePurchaseOrderLine;
using Energy.Application.Procurement.PurchaseOrderLine.Commands.DeletePurchaseOrderLine;
using Energy.Application.Procurement.PurchaseOrderLine.Commands.UpdatePurchaseOrderLine;
using Energy.Application.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineById;
using Energy.Application.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineList;
using Energy.Application.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// PurchaseOrderLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/purchase-order-lines")]
public sealed class PurchaseOrderLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseOrderLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>>> GetList([FromQuery] GetPurchaseOrderLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PurchaseOrderLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePurchaseOrderLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePurchaseOrderLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePurchaseOrderLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePurchaseOrderLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePurchaseOrderLineCommand(id), ct));
}
