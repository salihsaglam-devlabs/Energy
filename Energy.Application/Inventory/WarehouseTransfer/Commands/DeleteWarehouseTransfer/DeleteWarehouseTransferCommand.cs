using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransfer.Commands.DeleteWarehouseTransfer;

/// <summary>WarehouseTransfer kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWarehouseTransferCommand(Guid Id) : IRequest<BaseResponse<bool>>;
