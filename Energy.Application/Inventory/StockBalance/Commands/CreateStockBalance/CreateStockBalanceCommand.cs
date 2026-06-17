using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Commands.CreateStockBalance;

/// <summary>Yeni StockBalance oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockBalanceCommand(CreateStockBalanceRequest Request)
    : IRequest<BaseResponse<Guid>>;
