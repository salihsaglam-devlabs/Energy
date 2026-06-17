using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Commands.DeleteStockBalance;

/// <summary>StockBalance kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockBalanceCommand(Guid Id) : IRequest<BaseResponse<bool>>;
