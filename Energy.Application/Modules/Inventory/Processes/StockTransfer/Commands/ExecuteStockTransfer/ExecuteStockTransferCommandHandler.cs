using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;
using Energy.Application.Inventory.Services;
using MediatR;

namespace Energy.Application.Modules.Inventory.Processes.StockTransfer.Commands.ExecuteStockTransfer;

/// <summary><see cref="ExecuteStockTransferCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ExecuteStockTransferCommandHandler
    : IRequestHandler<ExecuteStockTransferCommand, BaseResponse<StockTransferProcessResponse>>
{
    private readonly IInventoryService _inventory;

    public ExecuteStockTransferCommandHandler(IInventoryService inventory)
    {
        _inventory = inventory;
    }

    public async Task<BaseResponse<StockTransferProcessResponse>> Handle(ExecuteStockTransferCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _inventory.TransferAsync(
                new StockTransferRequest(request.Request.SourceWarehouseId, request.Request.TargetWarehouseId, request.Request.MaterialId,
                    request.Request.UnitOfMeasureId, request.Request.Quantity, request.Request.Note), ct);
            return BaseResponse<StockTransferProcessResponse>.Success(
                new StockTransferProcessResponse { TotalCost = result.TotalCost, AllocationCount = result.Allocations.Count },
                "Completed");
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<StockTransferProcessResponse>.Failure(ex.Message);
        }
    }
}
