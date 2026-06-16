using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;
using Energy.Application.Procurement.Reports.PurchaseOrderSummary.Queries.GetPurchaseOrderSummaryData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.Procurement.Reports;

/// <summary>PurchaseOrderSummary raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/reports/purchase-order-summary")]
public sealed class PurchaseOrderSummaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseOrderSummaryController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>>> GetData([FromQuery] PurchaseOrderSummaryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPurchaseOrderSummaryDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] PurchaseOrderSummaryRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetPurchaseOrderSummaryDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "purchase-order-summary.csv");
    }
}
