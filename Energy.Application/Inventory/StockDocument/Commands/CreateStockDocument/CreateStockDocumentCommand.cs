using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Commands.CreateStockDocument;

/// <summary>Yeni StockDocument oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockDocumentCommand(CreateStockDocumentRequest Request)
    : IRequest<BaseResponse<Guid>>;
