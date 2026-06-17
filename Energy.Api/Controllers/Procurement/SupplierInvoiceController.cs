using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Procurement.SupplierInvoice.Commands.CreateSupplierInvoice;
using Energy.Application.Procurement.SupplierInvoice.Commands.DeleteSupplierInvoice;
using Energy.Application.Procurement.SupplierInvoice.Commands.UpdateSupplierInvoice;
using Energy.Application.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceById;
using Energy.Application.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceList;
using Energy.Application.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// SupplierInvoice uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/supplier-invoices")]
public sealed class SupplierInvoiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplierInvoiceController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>>> GetList([FromQuery] GetSupplierInvoiceListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierInvoiceListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SupplierInvoiceDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierInvoiceByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierInvoiceLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSupplierInvoiceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateSupplierInvoiceCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSupplierInvoiceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSupplierInvoiceCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteSupplierInvoiceCommand(id), ct));
}
