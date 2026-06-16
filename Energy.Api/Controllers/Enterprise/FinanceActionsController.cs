using Asp.Versioning;
using Energy.Application.Finance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Enterprise;

/// <summary>
/// Finance iş kuralı eylemleri: ödeme/tahsilat parçalı kapama, puantaj maliyet
/// hareketi, hakediş alacak/borç üretimi ve bütçe aşımı kontrolü. Yetkilendirme uç
/// nokta-permission eşlemesiyle uygulanır.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance-actions")]
public sealed class FinanceActionsController : ControllerBase
{
    private readonly IFinanceService _finance;

    public FinanceActionsController(IFinanceService finance) => _finance = finance;

    [HttpPost("allocate-payment/{paymentId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> AllocatePayment(
        Guid paymentId, [FromBody] List<FinanceAllocationLine> allocations, CancellationToken ct)
    {
        await _finance.AllocatePaymentAsync(paymentId, allocations, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    [HttpPost("allocate-collection/{collectionId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> AllocateCollection(
        Guid collectionId, [FromBody] List<FinanceAllocationLine> allocations, CancellationToken ct)
    {
        await _finance.AllocateCollectionAsync(collectionId, allocations, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    [HttpPost("timesheet-cost/{timesheetId:guid}")]
    public async Task<ActionResult<BaseResponse<Guid>>> TimesheetCost(
        Guid timesheetId, [FromQuery] Guid currencyId, CancellationToken ct)
        => Ok(BaseResponse<Guid>.Success(await _finance.PostTimesheetCostAsync(timesheetId, currencyId, ct)));

    [HttpPost("progress-payment/{progressPaymentId:guid}")]
    public async Task<ActionResult<BaseResponse<Guid>>> ProgressPayment(Guid progressPaymentId, CancellationToken ct)
        => Ok(BaseResponse<Guid>.Success(await _finance.PostProgressPaymentAsync(progressPaymentId, ct)));

    [HttpPost("budget-overrun/{budgetId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> BudgetOverrun(Guid budgetId, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _finance.CheckBudgetOverrunAsync(budgetId, ct)));
}

