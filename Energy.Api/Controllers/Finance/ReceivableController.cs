using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Finance.Receivable.Commands.CreateReceivable;
using Energy.Application.Finance.Receivable.Commands.DeleteReceivable;
using Energy.Application.Finance.Receivable.Commands.UpdateReceivable;
using Energy.Application.Finance.Receivable.Queries.GetReceivableById;
using Energy.Application.Finance.Receivable.Queries.GetReceivableList;
using Energy.Application.Finance.Receivable.Queries.GetReceivableLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// Receivable uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/receivables")]
public sealed class ReceivableController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceivableController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ReceivableListResponse>>>> GetList([FromQuery] GetReceivableListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReceivableListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ReceivableDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReceivableByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReceivableLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateReceivableRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateReceivableCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateReceivableRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateReceivableCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteReceivableCommand(id), ct));
}
