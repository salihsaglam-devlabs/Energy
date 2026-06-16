using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Commands.CreateSupplierInvoiceLine;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Commands.DeleteSupplierInvoiceLine;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Commands.UpdateSupplierInvoiceLine;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineById;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineList;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// SupplierInvoiceLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/supplier-invoice-lines")]
public sealed class SupplierInvoiceLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplierInvoiceLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>>> GetList([FromQuery] GetSupplierInvoiceLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierInvoiceLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SupplierInvoiceLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierInvoiceLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierInvoiceLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSupplierInvoiceLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateSupplierInvoiceLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSupplierInvoiceLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSupplierInvoiceLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteSupplierInvoiceLineCommand(id), ct));
}
