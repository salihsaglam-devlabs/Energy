using Asp.Versioning;
using Energy.Application.Finance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Finance.Processes;

/// <summary>
/// Puantaj maliyet süreci uç noktası (standart süreç rotası, HR Cost akışı).
/// Transaction-güvenli <see cref="IFinanceService"/> içinde finansal maliyet
/// hareketi üretir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/processes/timesheet-cost")]
public sealed class TimesheetCostProcessController : ControllerBase
{
    private readonly IFinanceService _finance;

    public TimesheetCostProcessController(IFinanceService finance) => _finance = finance;

    /// <summary>Puantaj maliyet sürecini yürütür.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<TimesheetCostProcessResponse>>> Execute([FromBody] TimesheetCostProcessRequest request, CancellationToken ct)
    {
        try
        {
            var id = await _finance.PostTimesheetCostAsync(request.TimesheetId, request.CurrencyId, ct);
            return Ok(BaseResponse<TimesheetCostProcessResponse>.Success(
                new TimesheetCostProcessResponse { FinancialTransactionId = id }, "Completed"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<TimesheetCostProcessResponse>.Failure(ex.Message));
        }
    }
}

