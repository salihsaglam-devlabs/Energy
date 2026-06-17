using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Commands.DeleteStockDocument;

/// <summary>StockDocument kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockDocumentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
