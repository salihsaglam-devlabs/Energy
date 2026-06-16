using Asp.Versioning;
using Energy.Application.Inventory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Inventory.Processes;

/// <summary>
/// Depolar arası stok transfer süreci uç noktası (standart süreç rotası). Kaynak
/// FIFO çıkış + hedef giriş transaction-güvenli <see cref="IInventoryService"/>
/// içinde tek işlemde yürür.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/processes/stock-transfer")]
public sealed class StockTransferProcessController : ControllerBase
{
    private readonly IInventoryService _inventory;

    public StockTransferProcessController(IInventoryService inventory) => _inventory = inventory;

    /// <summary>Stok transfer sürecini yürütür.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<StockTransferProcessResponse>>> Execute([FromBody] StockTransferProcessRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _inventory.TransferAsync(
                new StockTransferRequest(request.SourceWarehouseId, request.TargetWarehouseId, request.MaterialId,
                    request.UnitOfMeasureId, request.Quantity, request.Note), ct);
            return Ok(BaseResponse<StockTransferProcessResponse>.Success(
                new StockTransferProcessResponse { TotalCost = result.TotalCost, AllocationCount = result.Allocations.Count },
                "Completed"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<StockTransferProcessResponse>.Failure(ex.Message));
        }
    }
}

