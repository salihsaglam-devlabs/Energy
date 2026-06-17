using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;
using Energy.Application.Inventory.Services;
using MediatR;

namespace Energy.Application.Inventory.Processes.StockIssue.Commands.ExecuteStockIssue;

/// <summary><see cref="ExecuteStockIssueCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ExecuteStockIssueCommandHandler
    : IRequestHandler<ExecuteStockIssueCommand, BaseResponse<StockIssueProcessResponse>>
{
    private readonly IInventoryService _inventory;

    public ExecuteStockIssueCommandHandler(IInventoryService inventory)
    {
        _inventory = inventory;
    }

    public async Task<BaseResponse<StockIssueProcessResponse>> Handle(ExecuteStockIssueCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _inventory.PostStockOutAsync(
                new StockOutRequest(request.Request.WarehouseId, request.Request.MaterialId, request.Request.UnitOfMeasureId,
                    request.Request.Quantity, request.Request.ProjectId, request.Request.Note), ct);
            return BaseResponse<StockIssueProcessResponse>.Success(
                new StockIssueProcessResponse { TotalCost = result.TotalCost, AllocationCount = result.Allocations.Count },
                "Completed");
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<StockIssueProcessResponse>.Failure(ex.Message);
        }
    }
}
