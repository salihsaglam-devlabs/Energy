using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestApprover.Commands.UpdateApprovalRequestApprover;

/// <summary>Var olan ApprovalRequestApprover kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalRequestApproverCommand(Guid Id, UpdateApprovalRequestApproverRequest Request)
    : IRequest<BaseResponse<bool>>;
