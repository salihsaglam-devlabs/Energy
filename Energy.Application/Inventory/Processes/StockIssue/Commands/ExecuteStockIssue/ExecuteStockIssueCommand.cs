using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;
using MediatR;

namespace Energy.Application.Inventory.Processes.StockIssue.Commands.ExecuteStockIssue;

/// <summary>ExecuteStockIssue</summary>
public sealed record ExecuteStockIssueCommand(StockIssueProcessRequest Request)
    : IRequest<BaseResponse<StockIssueProcessResponse>>;
