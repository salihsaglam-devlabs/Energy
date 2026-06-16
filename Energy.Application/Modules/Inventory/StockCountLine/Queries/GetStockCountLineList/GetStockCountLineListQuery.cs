using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineList;

/// <summary>Sayfalanmış StockCountLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockCountLineListQuery(GetStockCountLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockCountLineListResponse>>>;
