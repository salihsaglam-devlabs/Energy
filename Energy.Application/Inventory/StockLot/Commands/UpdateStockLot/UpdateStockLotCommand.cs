using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Commands.UpdateStockLot;

/// <summary>Var olan StockLot kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockLotCommand(Guid Id, UpdateStockLotRequest Request)
    : IRequest<BaseResponse<bool>>;
