using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;
using MediatR;

namespace Energy.Application.Inventory.Processes.StockTransfer.Commands.ExecuteStockTransfer;

/// <summary>ExecuteStockTransfer</summary>
public sealed record ExecuteStockTransferCommand(StockTransferProcessRequest Request)
    : IRequest<BaseResponse<StockTransferProcessResponse>>;
