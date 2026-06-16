using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockReservation.Queries.GetStockReservationList;

/// <summary>Sayfalanmış StockReservation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetStockReservationListQuery(GetStockReservationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockReservationListResponse>>>;
