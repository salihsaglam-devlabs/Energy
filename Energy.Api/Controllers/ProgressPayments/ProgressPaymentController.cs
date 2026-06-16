using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.ProgressPayments.ProgressPayment.Commands.CreateProgressPayment;
using Energy.Application.ProgressPayments.ProgressPayment.Commands.DeleteProgressPayment;
using Energy.Application.ProgressPayments.ProgressPayment.Commands.UpdateProgressPayment;
using Energy.Application.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentById;
using Energy.Application.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentList;
using Energy.Application.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

namespace Energy.Api.Controllers.ProgressPayments;

/// <summary>
/// ProgressPayment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/progress-payments/progress-payments")]
public sealed class ProgressPaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressPaymentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>>> GetList([FromQuery] GetProgressPaymentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProgressPaymentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProgressPaymentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProgressPaymentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProgressPaymentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProgressPaymentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProgressPaymentCommand(id), ct));
}
