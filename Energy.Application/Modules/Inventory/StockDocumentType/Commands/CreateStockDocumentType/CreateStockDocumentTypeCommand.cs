using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Commands.CreateStockDocumentType;

/// <summary>Yeni StockDocumentType oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockDocumentTypeCommand(CreateStockDocumentTypeRequest Request)
    : IRequest<BaseResponse<Guid>>;
