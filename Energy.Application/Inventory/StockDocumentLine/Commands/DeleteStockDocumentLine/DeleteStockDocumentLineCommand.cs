using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentLine.Commands.DeleteStockDocumentLine;

/// <summary>StockDocumentLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockDocumentLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
