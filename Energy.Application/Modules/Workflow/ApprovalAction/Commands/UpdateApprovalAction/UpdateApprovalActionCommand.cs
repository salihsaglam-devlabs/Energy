using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Commands.UpdateApprovalAction;

/// <summary>Var olan ApprovalAction kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalActionCommand(Guid Id, UpdateApprovalActionRequest Request)
    : IRequest<BaseResponse<bool>>;
