using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Commands.UpdateStockDocumentType;

/// <summary>Var olan StockDocumentType kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockDocumentTypeCommand(Guid Id, UpdateStockDocumentTypeRequest Request)
    : IRequest<BaseResponse<bool>>;
