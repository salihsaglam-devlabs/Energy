using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.Reports.StockBalanceReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;

namespace Energy.Api.Controllers.Modules.Inventory.Reports;

/// <summary>StockBalanceReport raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/reports/stock-balance-report")]
public sealed class StockBalanceReportController : ControllerBase
{
    private readonly IStockBalanceReportService _service;

    public StockBalanceReportController(IStockBalanceReportService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>>> GetData([FromQuery] StockBalanceReportRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] StockBalanceReportRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _service.GetDataAsync(request, ct);
        var rows = result.Data?.Items ?? [];
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[] { "WarehouseId","MaterialId","Quantity","ReservedQuantity","TotalCost","LastRecalculatedAt" }));
        foreach (var r in rows)
        {
            sb.AppendLine(r.WarehouseId.ToString() + "," + r.MaterialId.ToString() + "," + r.Quantity.ToString() + "," + r.ReservedQuantity.ToString() + "," + r.TotalCost.ToString() + "," + r.LastRecalculatedAt.ToString());
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "stock-balance-report.csv");
    }
}
