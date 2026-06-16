using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Queries.GetStockLotList;

/// <summary>Sayfalanmış StockLot listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockLotListQuery(GetStockLotListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockLotListResponse>>>;
