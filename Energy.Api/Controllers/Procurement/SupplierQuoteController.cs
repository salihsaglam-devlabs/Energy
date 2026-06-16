using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Procurement.SupplierQuote.Commands.CreateSupplierQuote;
using Energy.Application.Modules.Procurement.SupplierQuote.Commands.DeleteSupplierQuote;
using Energy.Application.Modules.Procurement.SupplierQuote.Commands.UpdateSupplierQuote;
using Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteById;
using Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteList;
using Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

namespace Energy.Api.Controllers.Procurement;

/// <summary>
/// SupplierQuote uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/supplier-quotes")]
public sealed class SupplierQuoteController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplierQuoteController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>>> GetList([FromQuery] GetSupplierQuoteListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierQuoteListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SupplierQuoteDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierQuoteByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierQuoteLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSupplierQuoteRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateSupplierQuoteCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSupplierQuoteRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSupplierQuoteCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteSupplierQuoteCommand(id), ct));
}
