using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Commands.UpdateWorkOrderChecklist;

/// <summary>Var olan WorkOrderChecklist kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderChecklistCommand(Guid Id, UpdateWorkOrderChecklistRequest Request)
    : IRequest<BaseResponse<bool>>;
