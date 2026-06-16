using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Commands.DeleteStockCountLine;

/// <summary>StockCountLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockCountLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
