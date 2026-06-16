using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Web.Clients.Procurement.Reports.PurchaseOrderSummary;

namespace Energy.Web.Controllers.Procurement.Reports;

/// <summary>PurchaseOrderSummary rapor ekran denetleyicisi (yalnızca API istemcisiyle konuşur, salt-okunur).</summary>
[Authorize]
[Route("procurement/reports/purchase-order-summary")]
public sealed class PurchaseOrderSummaryController : Controller
{
    private readonly IPurchaseOrderSummaryApiClient _api;

    public PurchaseOrderSummaryController(IPurchaseOrderSummaryApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Procurement/Reports/PurchaseOrderSummary/Index.cshtml");

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
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses.PurchaseOrderSummaryRowResponse>(), totalCount = page?.TotalCount ?? 0 });
    }
}
