using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Finance.Payment.Commands.CreatePayment;
using Energy.Application.Finance.Payment.Commands.DeletePayment;
using Energy.Application.Finance.Payment.Commands.UpdatePayment;
using Energy.Application.Finance.Payment.Queries.GetPaymentById;
using Energy.Application.Finance.Payment.Queries.GetPaymentList;
using Energy.Application.Finance.Payment.Queries.GetPaymentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using Energy.Shared.Models.V1.Finance.Payment.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// Payment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/payments")]
public sealed class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PaymentListResponse>>>> GetList([FromQuery] GetPaymentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPaymentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PaymentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPaymentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PaymentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPaymentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePaymentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePaymentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePaymentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePaymentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePaymentCommand(id), ct));
}
