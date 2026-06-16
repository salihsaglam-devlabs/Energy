using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Queries.GetStockBalanceList;

/// <summary>Sayfalanmış StockBalance listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockBalanceListQuery(GetStockBalanceListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockBalanceListResponse>>>;
