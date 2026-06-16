using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Procurement.PurchaseReceiptLine.Commands.CreatePurchaseReceiptLine;
using Energy.Application.Procurement.PurchaseReceiptLine.Commands.DeletePurchaseReceiptLine;
using Energy.Application.Procurement.PurchaseReceiptLine.Commands.UpdatePurchaseReceiptLine;
using Energy.Application.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineById;
using Energy.Application.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineList;
using Energy.Application.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// PurchaseReceiptLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/purchase-receipt-lines")]
public sealed class PurchaseReceiptLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseReceiptLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>>> GetList([FromQuery] GetPurchaseReceiptLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseReceiptLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PurchaseReceiptLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseReceiptLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseReceiptLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePurchaseReceiptLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePurchaseReceiptLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePurchaseReceiptLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePurchaseReceiptLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePurchaseReceiptLineCommand(id), ct));
}
