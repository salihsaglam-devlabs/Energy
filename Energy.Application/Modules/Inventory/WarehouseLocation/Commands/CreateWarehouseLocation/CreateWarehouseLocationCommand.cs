using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseLocation.Commands.CreateWarehouseLocation;

/// <summary>Yeni WarehouseLocation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWarehouseLocationCommand(CreateWarehouseLocationRequest Request)
    : IRequest<BaseResponse<Guid>>;
