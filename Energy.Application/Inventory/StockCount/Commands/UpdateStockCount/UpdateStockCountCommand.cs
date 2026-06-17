using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockCount.Commands.UpdateStockCount;

/// <summary>Var olan StockCount kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockCountCommand(Guid Id, UpdateStockCountRequest Request)
    : IRequest<BaseResponse<bool>>;
