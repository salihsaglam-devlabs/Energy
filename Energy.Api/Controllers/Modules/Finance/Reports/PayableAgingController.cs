using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.Reports.PayableAging.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;

namespace Energy.Api.Controllers.Modules.Finance.Reports;

/// <summary>PayableAging raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/reports/payable-aging")]
public sealed class PayableAgingController : ControllerBase
{
    private readonly IPayableAgingService _service;

    public PayableAgingController(IPayableAgingService service) => _service = service;

    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>>> GetData([FromQuery] PayableAgingRequest request, CancellationToken ct)
        => Ok(await _service.GetDataAsync(request, ct));

    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] PayableAgingRequest request, CancellationToken ct)
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
        return File(bytes, "text/csv", "payable-aging.csv");
    }
}
