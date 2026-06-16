using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Commands.DeleteWarehouseLocation;

/// <summary>WarehouseLocation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWarehouseLocationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
