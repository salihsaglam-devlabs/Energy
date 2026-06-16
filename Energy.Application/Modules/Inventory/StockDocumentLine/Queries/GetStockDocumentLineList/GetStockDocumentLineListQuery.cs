using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineList;

/// <summary>Sayfalanmış StockDocumentLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockDocumentLineListQuery(GetStockDocumentLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>>;
