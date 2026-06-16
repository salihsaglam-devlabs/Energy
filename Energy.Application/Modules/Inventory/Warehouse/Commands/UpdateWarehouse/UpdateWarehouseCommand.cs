using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.Warehouse.Commands.UpdateWarehouse;

/// <summary>Var olan Warehouse kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWarehouseCommand(Guid Id, UpdateWarehouseRequest Request)
    : IRequest<BaseResponse<bool>>;
