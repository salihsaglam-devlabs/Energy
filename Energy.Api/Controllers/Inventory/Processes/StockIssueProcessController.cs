using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;
using Energy.Application.Modules.Inventory.Processes.StockIssue.Commands.ExecuteStockIssue;

namespace Energy.Api.Controllers.Inventory.Processes;

/// <summary>Stok çıkış süreci (FIFO maliyetlendirme + stok hareketi).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/processes/stock-issue")]
public sealed class StockIssueProcessController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockIssueProcessController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<StockIssueProcessResponse>>> Execute([FromBody] StockIssueProcessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ExecuteStockIssueCommand(request), ct));
}
