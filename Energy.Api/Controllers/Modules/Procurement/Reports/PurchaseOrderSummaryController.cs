using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Procurement.Reports.PurchaseOrderSummary.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;

namespace Energy.Api.Controllers.Modules.Procurement.Reports;

/// <summary>PurchaseOrderSummary raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/reports/purchase-order-summary")]
public sealed class PurchaseOrderSummaryController : ControllerBase
{
    private readonly IPurchaseOrderSummaryService _service;

    public PurchaseOrderSummaryController(IPurchaseOrderSummaryService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>>> GetData([FromQuery] PurchaseOrderSummaryRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] PurchaseOrderSummaryRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _service.GetDataAsync(request, ct);
        var rows = result.Data?.Items ?? [];
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[] { "OrderNo","OrderDate","SupplierId","ProjectId","CurrencyId","Status" }));
        foreach (var r in rows)
        {
            sb.AppendLine((r.OrderNo ?? string.Empty) + "," + r.OrderDate.ToString() + "," + r.SupplierId.ToString() + "," + r.ProjectId.ToString() + "," + r.CurrencyId.ToString() + "," + (r.Status ?? string.Empty));
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "purchase-order-summary.csv");
    }
}
