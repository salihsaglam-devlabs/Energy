using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Procurement.PurchaseReceipt.Commands.CreatePurchaseReceipt;
using Energy.Application.Procurement.PurchaseReceipt.Commands.DeletePurchaseReceipt;
using Energy.Application.Procurement.PurchaseReceipt.Commands.UpdatePurchaseReceipt;
using Energy.Application.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptById;
using Energy.Application.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptList;
using Energy.Application.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// PurchaseReceipt uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/purchase-receipts")]
public sealed class PurchaseReceiptController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseReceiptController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>>> GetList([FromQuery] GetPurchaseReceiptListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseReceiptListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PurchaseReceiptDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseReceiptByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseReceiptLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePurchaseReceiptRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePurchaseReceiptCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePurchaseReceiptRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePurchaseReceiptCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePurchaseReceiptCommand(id), ct));
}
