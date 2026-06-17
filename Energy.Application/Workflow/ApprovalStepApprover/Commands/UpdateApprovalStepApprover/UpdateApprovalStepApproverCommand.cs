using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Commands.UpdateApprovalStepApprover;

/// <summary>Var olan ApprovalStepApprover kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalStepApproverCommand(Guid Id, UpdateApprovalStepApproverRequest Request)
    : IRequest<BaseResponse<bool>>;
