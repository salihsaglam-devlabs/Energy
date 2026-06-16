using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.ProgressPayments.Reports.ProgressPaymentSummary.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;

namespace Energy.Api.Controllers.Modules.ProgressPayments.Reports;

/// <summary>ProgressPaymentSummary raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/progress-payments/reports/progress-payment-summary")]
public sealed class ProgressPaymentSummaryController : ControllerBase
{
    private readonly IProgressPaymentSummaryService _service;

    public ProgressPaymentSummaryController(IProgressPaymentSummaryService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>>> GetData([FromQuery] ProgressPaymentSummaryRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] ProgressPaymentSummaryRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _service.GetDataAsync(request, ct);
        var rows = result.Data?.Items ?? [];
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[] { "ProgressPaymentNo","ContractId","GrossAmount","NetAmount","PaymentPeriodStart","Status" }));
        foreach (var r in rows)
        {
            sb.AppendLine((r.ProgressPaymentNo ?? string.Empty) + "," + r.ContractId.ToString() + "," + r.GrossAmount.ToString() + "," + r.NetAmount.ToString() + "," + r.PaymentPeriodStart.ToString() + "," + (r.Status ?? string.Empty));
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "progress-payment-summary.csv");
    }
}
