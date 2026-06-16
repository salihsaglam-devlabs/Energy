using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationList;

/// <summary>Sayfalanmış StockIssueAllocation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockIssueAllocationListQuery(GetStockIssueAllocationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>>;
