using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Commands.UpdateStockDocumentLine;

/// <summary>Var olan StockDocumentLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateStockDocumentLineCommand(Guid Id, UpdateStockDocumentLineRequest Request)
    : IRequest<BaseResponse<bool>>;
