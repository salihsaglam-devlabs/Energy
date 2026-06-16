using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Commands.DeleteWorkOrderChecklistItem;

/// <summary>WorkOrderChecklistItem kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderChecklistItemCommand(Guid Id) : IRequest<BaseResponse<bool>>;
