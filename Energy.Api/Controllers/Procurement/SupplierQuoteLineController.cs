using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Procurement.SupplierQuoteLine.Commands.CreateSupplierQuoteLine;
using Energy.Application.Procurement.SupplierQuoteLine.Commands.DeleteSupplierQuoteLine;
using Energy.Application.Procurement.SupplierQuoteLine.Commands.UpdateSupplierQuoteLine;
using Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineById;
using Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineList;
using Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// SupplierQuoteLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/supplier-quote-lines")]
public sealed class SupplierQuoteLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplierQuoteLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>>> GetList([FromQuery] GetSupplierQuoteLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierQuoteLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SupplierQuoteLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierQuoteLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierQuoteLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSupplierQuoteLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateSupplierQuoteLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSupplierQuoteLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSupplierQuoteLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteSupplierQuoteLineCommand(id), ct));
}
