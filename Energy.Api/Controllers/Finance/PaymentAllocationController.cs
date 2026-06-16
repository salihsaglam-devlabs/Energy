using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.PaymentAllocation.Commands.CreatePaymentAllocation;
using Energy.Application.Modules.Finance.PaymentAllocation.Commands.DeletePaymentAllocation;
using Energy.Application.Modules.Finance.PaymentAllocation.Commands.UpdatePaymentAllocation;
using Energy.Application.Modules.Finance.PaymentAllocation.Queries.GetPaymentAllocationById;
using Energy.Application.Modules.Finance.PaymentAllocation.Queries.GetPaymentAllocationList;
using Energy.Application.Modules.Finance.PaymentAllocation.Queries.GetPaymentAllocationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// PaymentAllocation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/payment-allocations")]
public sealed class PaymentAllocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentAllocationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>>> GetList([FromQuery] GetPaymentAllocationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPaymentAllocationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PaymentAllocationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPaymentAllocationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPaymentAllocationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePaymentAllocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePaymentAllocationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePaymentAllocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePaymentAllocationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePaymentAllocationCommand(id), ct));
}
