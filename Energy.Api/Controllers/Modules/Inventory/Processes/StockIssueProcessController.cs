using Asp.Versioning;
using Energy.Application.Inventory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Inventory.Processes;

/// <summary>
/// Stok çıkış süreci uç noktası (standart süreç rotası). FIFO maliyetlendirme +
/// StockTransaction + StockBalance güncellemesi transaction-güvenli
/// <see cref="IInventoryService"/> içinde yürür.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/processes/stock-issue")]
public sealed class StockIssueProcessController : ControllerBase
{
    private readonly IInventoryService _inventory;

    public StockIssueProcessController(IInventoryService inventory) => _inventory = inventory;

    /// <summary>Stok çıkış sürecini yürütür.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<StockIssueProcessResponse>>> Execute([FromBody] StockIssueProcessRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _inventory.PostStockOutAsync(
                new StockOutRequest(request.WarehouseId, request.MaterialId, request.UnitOfMeasureId,
                    request.Quantity, request.ProjectId, request.Note), ct);
            return Ok(BaseResponse<StockIssueProcessResponse>.Success(
                new StockIssueProcessResponse { TotalCost = result.TotalCost, AllocationCount = result.Allocations.Count },
                "Completed"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<StockIssueProcessResponse>.Failure(ex.Message));
        }
    }
}

