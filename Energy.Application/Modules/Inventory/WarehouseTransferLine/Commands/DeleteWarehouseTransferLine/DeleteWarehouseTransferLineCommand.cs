using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransferLine.Commands.DeleteWarehouseTransferLine;

/// <summary>WarehouseTransferLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWarehouseTransferLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
