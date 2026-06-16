using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Commands.DeleteStockReservation;

/// <summary>StockReservation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockReservationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
