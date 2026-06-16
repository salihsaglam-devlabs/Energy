using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Commands.DeleteStockTransaction;

/// <summary>StockTransaction kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockTransactionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
