using Asp.Versioning;
using Energy.Application.Finance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Finance.Processes;

/// <summary>
/// Ödeme tahsis süreci uç noktası (standart süreç rotası, Finance akışı). Bir
/// ödemeyi birden çok borca parçalı kapatır; tahsis + finansal hareketler
/// transaction-güvenli <see cref="IFinanceService"/> içinde üretilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/processes/payment-allocation")]
public sealed class PaymentAllocationProcessController : ControllerBase
{
    private readonly IFinanceService _finance;

    public PaymentAllocationProcessController(IFinanceService finance) => _finance = finance;

    /// <summary>Ödeme tahsis sürecini yürütür.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<PaymentAllocationProcessResponse>>> Execute([FromBody] PaymentAllocationProcessRequest request, CancellationToken ct)
    {
        if (request.Lines is null || request.Lines.Count == 0)
        {
            return BadRequest(BaseResponse<PaymentAllocationProcessResponse>.Failure("At least one allocation line is required."));
        }

        try
        {
            var lines = request.Lines
                .Select(l => new FinanceAllocationLine(l.TargetId, l.Amount))
                .ToList();
            await _finance.AllocatePaymentAsync(request.PaymentId, lines, ct);
            return Ok(BaseResponse<PaymentAllocationProcessResponse>.Success(
                new PaymentAllocationProcessResponse
                {
                    AllocatedLineCount = lines.Count,
                    TotalAllocated = lines.Sum(l => l.Amount),
                }, "Completed"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<PaymentAllocationProcessResponse>.Failure(ex.Message));
        }
    }
}

