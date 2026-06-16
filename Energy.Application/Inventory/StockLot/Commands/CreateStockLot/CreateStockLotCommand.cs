using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Commands.CreateStockLot;

/// <summary>Yeni StockLot oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockLotCommand(CreateStockLotRequest Request)
    : IRequest<BaseResponse<Guid>>;
