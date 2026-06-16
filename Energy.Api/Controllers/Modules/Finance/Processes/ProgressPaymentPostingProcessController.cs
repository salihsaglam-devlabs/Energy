using Asp.Versioning;
using Energy.Application.Finance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Finance.Processes;

/// <summary>
/// Hakediş muhasebeleştirme süreci uç noktası (standart süreç rotası, Contracts
/// akışı). Transaction-güvenli <see cref="IFinanceService"/> içinde alacak/borç
/// finansal hareketi üretir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/processes/progress-payment-posting")]
public sealed class ProgressPaymentPostingProcessController : ControllerBase
{
    private readonly IFinanceService _finance;

    public ProgressPaymentPostingProcessController(IFinanceService finance) => _finance = finance;

    /// <summary>Hakediş muhasebeleştirme sürecini yürütür.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<ProgressPaymentPostingProcessResponse>>> Execute([FromBody] ProgressPaymentPostingProcessRequest request, CancellationToken ct)
    {
        try
        {
            var id = await _finance.PostProgressPaymentAsync(request.ProgressPaymentId, ct);
            return Ok(BaseResponse<ProgressPaymentPostingProcessResponse>.Success(
                new ProgressPaymentPostingProcessResponse { FinancialTransactionId = id }, "Completed"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<ProgressPaymentPostingProcessResponse>.Failure(ex.Message));
        }
    }
}

