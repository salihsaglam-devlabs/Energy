using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;
using Energy.Application.Modules.Procurement.Processes.GoodsReceipt.Commands.ExecuteGoodsReceipt;

namespace Energy.Api.Controllers.Procurement.Processes;

/// <summary>Mal kabul süreci (irsaliyeyi stok girişine dönüştürür).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/processes/goods-receipt")]
public sealed class GoodsReceiptProcessController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoodsReceiptProcessController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> Execute([FromBody] GoodsReceiptProcessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ExecuteGoodsReceiptCommand(request), ct));
}
