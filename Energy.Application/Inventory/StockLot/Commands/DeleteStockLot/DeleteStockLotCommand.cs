using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Commands.DeleteStockLot;

/// <summary>StockLot kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockLotCommand(Guid Id) : IRequest<BaseResponse<bool>>;
