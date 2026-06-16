using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Commands.CreateStockReservation;

/// <summary>Yeni StockReservation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockReservationCommand(CreateStockReservationRequest Request)
    : IRequest<BaseResponse<Guid>>;
