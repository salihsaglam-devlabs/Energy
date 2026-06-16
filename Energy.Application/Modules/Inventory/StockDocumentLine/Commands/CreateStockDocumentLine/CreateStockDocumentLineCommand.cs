using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Commands.CreateStockDocumentLine;

/// <summary>Yeni StockDocumentLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockDocumentLineCommand(CreateStockDocumentLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
