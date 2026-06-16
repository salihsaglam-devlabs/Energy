using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;
using Energy.Application.Modules.Finance.Reports.PayableAging.Queries.GetPayableAgingData;
using Energy.Api.Common.Export;

namespace Energy.Api.Controllers.Finance.Reports;

/// <summary>PayableAging raporu uç noktaları (veri + export). Salt-okunur.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/reports/payable-aging")]
public sealed class PayableAgingController : ControllerBase
{
    private readonly IMediator _mediator;

    public PayableAgingController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>>> GetData([FromQuery] PayableAgingRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPayableAgingDataQuery(request), ct));

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] PayableAgingRequest request, CancellationToken ct)
    {
        request.PageNumber = 1;
        request.PageSize = 100000;
        var result = await _mediator.Send(new GetPayableAgingDataQuery(request), ct);
        return File(CsvExport.ToBytes(result.Data?.Items), "text/csv", "payable-aging.csv");
    }
}
