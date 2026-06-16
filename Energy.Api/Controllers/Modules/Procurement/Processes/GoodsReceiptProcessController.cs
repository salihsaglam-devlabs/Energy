using Asp.Versioning;
using Energy.Application.Procurement.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Procurement.Processes;

/// <summary>
/// Mal kabul süreci uç noktası (standart süreç rotası). Onaylı satınalma
/// irsaliyesini stok girişine dönüştürür (StockDocument + StockLot +
/// StockTransaction + StockBalance), transaction-güvenli
/// <see cref="IGoodsReceiptService"/> içinde.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/processes/goods-receipt")]
public sealed class GoodsReceiptProcessController : ControllerBase
{
    private readonly IGoodsReceiptService _goodsReceipt;

    public GoodsReceiptProcessController(IGoodsReceiptService goodsReceipt) => _goodsReceipt = goodsReceipt;

    /// <summary>Mal kabul sürecini yürütür (irsaliyeyi stok girişine dönüştürür).</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> Execute([FromBody] GoodsReceiptProcessRequest request, CancellationToken ct)
    {
        try
        {
            await _goodsReceipt.ReceiveAsync(request.PurchaseReceiptId, ct);
            return Ok(BaseResponse<bool>.Success(true, "Completed"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<bool>.Failure(ex.Message));
        }
    }
}

