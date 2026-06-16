using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountList;

/// <summary>Sayfalanmış StockCount listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockCountListQuery(GetStockCountListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockCountListResponse>>>;
