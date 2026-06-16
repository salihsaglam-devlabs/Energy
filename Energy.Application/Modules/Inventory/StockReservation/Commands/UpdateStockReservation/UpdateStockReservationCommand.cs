using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Commands.UpdateStockReservation;

/// <summary>Var olan StockReservation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockReservationCommand(Guid Id, UpdateStockReservationRequest Request)
    : IRequest<BaseResponse<bool>>;
