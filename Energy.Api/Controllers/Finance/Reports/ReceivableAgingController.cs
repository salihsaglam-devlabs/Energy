using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;
using Energy.Application.Finance.Reports.ReceivableAging.Queries.GetReceivableAgingData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.Finance.Reports;

/// <summary>ReceivableAging raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/reports/receivable-aging")]
public sealed class ReceivableAgingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceivableAgingController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>>> GetData([FromQuery] ReceivableAgingRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReceivableAgingDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] ReceivableAgingRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetReceivableAgingDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "receivable-aging.csv");
    }
}
