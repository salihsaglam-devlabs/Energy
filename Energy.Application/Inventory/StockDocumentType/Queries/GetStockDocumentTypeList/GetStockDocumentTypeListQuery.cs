using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentType.Queries.GetStockDocumentTypeList;

/// <summary>Sayfalanmış StockDocumentType listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockDocumentTypeListQuery(GetStockDocumentTypeListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockDocumentTypeListResponse>>>;
