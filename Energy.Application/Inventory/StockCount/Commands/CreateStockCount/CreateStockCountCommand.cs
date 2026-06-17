using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockCount.Commands.CreateStockCount;

/// <summary>Yeni StockCount oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockCountCommand(CreateStockCountRequest Request)
    : IRequest<BaseResponse<Guid>>;
