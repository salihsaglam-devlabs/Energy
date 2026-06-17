using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockCount.Commands.DeleteStockCount;

/// <summary>StockCount kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockCountCommand(Guid Id) : IRequest<BaseResponse<bool>>;
