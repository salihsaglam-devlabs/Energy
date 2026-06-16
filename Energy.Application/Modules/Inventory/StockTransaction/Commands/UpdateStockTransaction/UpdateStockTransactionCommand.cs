using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Commands.UpdateStockTransaction;

/// <summary>Var olan StockTransaction kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockTransactionCommand(Guid Id, UpdateStockTransactionRequest Request)
    : IRequest<BaseResponse<bool>>;
