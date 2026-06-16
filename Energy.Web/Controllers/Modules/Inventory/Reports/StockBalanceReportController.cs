using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Web.Clients.Modules.Inventory.Reports.StockBalanceReport;

namespace Energy.Web.Controllers.Modules.Inventory.Reports;

/// <summary>StockBalanceReport rapor ekran denetleyicisi (yalnızca API istemcisiyle konuşur, salt-okunur).</summary>
[Authorize]
[Route("inventory/reports/stock-balance-report")]
public sealed class StockBalanceReportController : Controller
{
    private readonly IStockBalanceReportApiClient _api;

    public StockBalanceReportController(IStockBalanceReportApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Modules/Inventory/Reports/StockBalanceReport/Index.cshtml");

    [HttpGet("data")]
    public async Task<IActionResult> Data(int skip = 0, int take = 50, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var pageSize = take <= 0 ? 50 : take;
        var parts = new List<string> { $"PageNumber={pageNumber}", $"PageSize={pageSize}" };
        if (startDate.HasValue) parts.Add($"StartDate={startDate.Value:O}");
        if (endDate.HasValue) parts.Add($"EndDate={endDate.Value:O}");

        var envelope = await _api.GetDataAsync(string.Join("&", parts), ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses.StockBalanceReportRowResponse>(), totalCount = page?.TotalCount ?? 0 });
    }
}
