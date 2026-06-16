using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Commands.UpdateStockBalance;

/// <summary>Var olan StockBalance kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockBalanceCommand(Guid Id, UpdateStockBalanceRequest Request)
    : IRequest<BaseResponse<bool>>;
