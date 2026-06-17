using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Commands.UpdateWarehouseLocation;

/// <summary>Var olan WarehouseLocation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWarehouseLocationCommand(Guid Id, UpdateWarehouseLocationRequest Request)
    : IRequest<BaseResponse<bool>>;
