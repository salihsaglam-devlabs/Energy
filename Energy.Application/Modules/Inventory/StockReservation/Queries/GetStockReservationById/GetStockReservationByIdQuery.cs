using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationById;

/// <summary>Kimliğe göre StockReservation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockReservationByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockReservationDetailResponse>>;
