using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;
using MediatR;

namespace Energy.Application.Inventory.Warehouse.Commands.CreateWarehouse;

/// <summary>Yeni Warehouse oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWarehouseCommand(CreateWarehouseRequest Request)
    : IRequest<BaseResponse<Guid>>;
