using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;
using Energy.Application.Modules.Inventory.Processes.StockTransfer.Commands.ExecuteStockTransfer;

namespace Energy.Api.Controllers.Inventory.Processes;

/// <summary>Depolar arası stok transfer süreci (FIFO çıkış + giriş, tek işlem).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/processes/stock-transfer")]
public sealed class StockTransferProcessController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockTransferProcessController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<StockTransferProcessResponse>>> Execute([FromBody] StockTransferProcessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ExecuteStockTransferCommand(request), ct));
}
