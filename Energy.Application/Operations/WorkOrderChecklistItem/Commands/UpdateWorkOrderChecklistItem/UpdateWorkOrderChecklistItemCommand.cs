using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Commands.UpdateWorkOrderChecklistItem;

/// <summary>Var olan WorkOrderChecklistItem kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderChecklistItemCommand(Guid Id, UpdateWorkOrderChecklistItemRequest Request)
    : IRequest<BaseResponse<bool>>;
