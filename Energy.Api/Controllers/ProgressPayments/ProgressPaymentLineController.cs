using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.CreateProgressPaymentLine;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.DeleteProgressPaymentLine;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.UpdateProgressPaymentLine;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineById;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineList;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

namespace Energy.Api.Controllers.ProgressPayments;

/// <summary>
/// ProgressPaymentLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/progress-payments/progress-payment-lines")]
public sealed class ProgressPaymentLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressPaymentLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>>> GetList([FromQuery] GetProgressPaymentLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProgressPaymentLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProgressPaymentLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProgressPaymentLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProgressPaymentLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProgressPaymentLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProgressPaymentLineCommand(id), ct));
}
