using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.Reports.ReceivableAging.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;

namespace Energy.Api.Controllers.Modules.Finance.Reports;

/// <summary>ReceivableAging raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/reports/receivable-aging")]
public sealed class ReceivableAgingController : ControllerBase
{
    private readonly IReceivableAgingService _service;

    public ReceivableAgingController(IReceivableAgingService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>>> GetData([FromQuery] ReceivableAgingRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] ReceivableAgingRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _service.GetDataAsync(request, ct);
        var rows = result.Data?.Items ?? [];
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[] { "PartnerId","CurrencyId","Amount","RemainingAmount","DueDate","IsClosed" }));
        foreach (var r in rows)
        {
            sb.AppendLine(r.PartnerId.ToString() + "," + r.CurrencyId.ToString() + "," + r.Amount.ToString() + "," + r.RemainingAmount.ToString() + "," + r.DueDate.ToString() + "," + r.IsClosed.ToString());
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "receivable-aging.csv");
    }
}
