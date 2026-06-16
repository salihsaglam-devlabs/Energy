using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Web.Clients.Modules.ProgressPayments.Reports.ProgressPaymentSummary;

namespace Energy.Web.Controllers.Modules.ProgressPayments.Reports;

/// <summary>ProgressPaymentSummary rapor ekran denetleyicisi (yalnızca API istemcisiyle konuşur, salt-okunur).</summary>
[Authorize]
[Route("progress-payments/reports/progress-payment-summary")]
public sealed class ProgressPaymentSummaryController : Controller
{
    private readonly IProgressPaymentSummaryApiClient _api;

    public ProgressPaymentSummaryController(IProgressPaymentSummaryApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Modules/ProgressPayments/Reports/ProgressPaymentSummary/Index.cshtml");

    [HttpGet("data")]
    public async Task<IActionResult> Data(int skip = 0, int take = 50, DateTime? startDate = null, DateTime? endDate = null, string? status = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var pageSize = take <= 0 ? 50 : take;
        var parts = new List<string> { $"PageNumber={pageNumber}", $"PageSize={pageSize}" };
        if (startDate.HasValue) parts.Add($"StartDate={startDate.Value:O}");
        if (endDate.HasValue) parts.Add($"EndDate={endDate.Value:O}");
        if (!string.IsNullOrWhiteSpace(status)) parts.Add($"Status={Uri.EscapeDataString(status)}");
        var envelope = await _api.GetDataAsync(string.Join("&", parts), ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses.ProgressPaymentSummaryRowResponse>(), totalCount = page?.TotalCount ?? 0 });
    }
}
