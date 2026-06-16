using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Commands.CreateStockTransaction;

/// <summary>Yeni StockTransaction oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockTransactionCommand(CreateStockTransactionRequest Request)
    : IRequest<BaseResponse<Guid>>;
