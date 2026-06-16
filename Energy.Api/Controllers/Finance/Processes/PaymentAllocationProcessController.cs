using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;
using Energy.Application.Finance.Processes.PaymentAllocation.Commands.ExecutePaymentAllocation;

namespace Energy.Api.Controllers.Finance.Processes;

/// <summary>Ödeme tahsis süreci (bir ödemeyi birden çok borca kapatır).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/processes/payment-allocation")]
public sealed class PaymentAllocationProcessController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentAllocationProcessController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<PaymentAllocationProcessResponse>>> Execute([FromBody] PaymentAllocationProcessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ExecutePaymentAllocationCommand(request), ct));
}
