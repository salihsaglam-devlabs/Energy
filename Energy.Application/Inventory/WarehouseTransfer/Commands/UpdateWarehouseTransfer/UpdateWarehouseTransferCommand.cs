using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransfer.Commands.UpdateWarehouseTransfer;

/// <summary>Var olan WarehouseTransfer kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWarehouseTransferCommand(Guid Id, UpdateWarehouseTransferRequest Request)
    : IRequest<BaseResponse<bool>>;
