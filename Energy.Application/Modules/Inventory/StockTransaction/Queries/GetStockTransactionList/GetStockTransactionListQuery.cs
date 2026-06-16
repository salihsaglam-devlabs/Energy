using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Queries.GetStockTransactionList;

/// <summary>Sayfalanmış StockTransaction listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockTransactionListQuery(GetStockTransactionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockTransactionListResponse>>>;
