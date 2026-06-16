using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocument.Queries.GetStockDocumentList;

/// <summary>Sayfalanmış StockDocument listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockDocumentListQuery(GetStockDocumentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockDocumentListResponse>>>;
