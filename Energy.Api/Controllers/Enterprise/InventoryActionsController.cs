using Asp.Versioning;
using Energy.Application.Inventory.Services;
using Energy.Application.Procurement.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Enterprise;

/// <summary>
/// Inventory iş kuralı eylemleri: stok girişi/çıkışı (FIFO), transfer, sayım düzeltmesi
/// ve bakiye yeniden üretimi. Yetkilendirme uç nokta-permission eşlemesiyle uygulanır.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory-actions")]
public sealed class InventoryActionsController : ControllerBase
{
    private readonly IInventoryService _inventory;

    public InventoryActionsController(IInventoryService inventory) => _inventory = inventory;

    [HttpPost("stock-in")]
    public async Task<ActionResult<BaseResponse<Guid>>> StockIn([FromBody] StockInRequest request, CancellationToken ct)
        => Ok(BaseResponse<Guid>.Success(await _inventory.PostStockInAsync(request, ct)));

    [HttpPost("stock-out")]
    public async Task<ActionResult<BaseResponse<StockIssueResult>>> StockOut([FromBody] StockOutRequest request, CancellationToken ct)
        => Ok(BaseResponse<StockIssueResult>.Success(await _inventory.PostStockOutAsync(request, ct)));

    [HttpPost("transfer")]
    public async Task<ActionResult<BaseResponse<StockIssueResult>>> Transfer([FromBody] StockTransferRequest request, CancellationToken ct)
        => Ok(BaseResponse<StockIssueResult>.Success(await _inventory.TransferAsync(request, ct)));

    public sealed record CountBody(Guid WarehouseId, Guid MaterialId, Guid UnitOfMeasureId, decimal CountedQuantity);

    [HttpPost("count")]
    public async Task<ActionResult<BaseResponse<StockCountAdjustmentResult>>> Count([FromBody] CountBody body, CancellationToken ct)
        => Ok(BaseResponse<StockCountAdjustmentResult>.Success(
            await _inventory.AdjustToCountAsync(body.WarehouseId, body.MaterialId, body.UnitOfMeasureId, body.CountedQuantity, ct)));

    public sealed record RebuildBody(Guid? WarehouseId, Guid? MaterialId);

    [HttpPost("rebuild-balances")]
    public async Task<ActionResult<BaseResponse<int>>> Rebuild([FromBody] RebuildBody body, CancellationToken ct)
        => Ok(BaseResponse<int>.Success(await _inventory.RebuildBalancesAsync(body.WarehouseId, body.MaterialId, ct)));

    [HttpPost("reverse/{stockDocumentId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Reverse(Guid stockDocumentId, CancellationToken ct)
    {
        await _inventory.ReverseDocumentAsync(stockDocumentId, null, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }
}

/// <summary>
/// Procurement iş kuralı eylemleri: mal kabulü stok girişine dönüştürür.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement-actions")]
public sealed class ProcurementActionsController : ControllerBase
{
    private readonly IGoodsReceiptService _goodsReceipt;

    public ProcurementActionsController(IGoodsReceiptService goodsReceipt) => _goodsReceipt = goodsReceipt;

    [HttpPost("receive/{purchaseReceiptId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Receive(Guid purchaseReceiptId, CancellationToken ct)
    {
        await _goodsReceipt.ReceiveAsync(purchaseReceiptId, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }
}

