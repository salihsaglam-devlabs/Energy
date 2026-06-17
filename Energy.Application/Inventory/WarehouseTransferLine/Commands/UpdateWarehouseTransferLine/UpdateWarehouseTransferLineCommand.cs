using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Commands.UpdateWarehouseTransferLine;

/// <summary>Var olan WarehouseTransferLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWarehouseTransferLineCommand(Guid Id, UpdateWarehouseTransferLineRequest Request)
    : IRequest<BaseResponse<bool>>;
