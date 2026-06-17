using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentType.Commands.DeleteStockDocumentType;

/// <summary>StockDocumentType kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockDocumentTypeCommand(Guid Id) : IRequest<BaseResponse<bool>>;
