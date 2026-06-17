using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;
using Energy.Application.Inventory.Reports.StockBalanceReport.Queries.GetStockBalanceReportData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.Inventory.Reports;

/// <summary>StockBalanceReport raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/reports/stock-balance-report")]
public sealed class StockBalanceReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockBalanceReportController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>>> GetData([FromQuery] StockBalanceReportRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockBalanceReportDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] StockBalanceReportRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetStockBalanceReportDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "stock-balance-report.csv");
    }
}
