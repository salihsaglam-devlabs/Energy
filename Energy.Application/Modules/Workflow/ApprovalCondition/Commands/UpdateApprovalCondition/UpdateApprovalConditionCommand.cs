using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Commands.UpdateApprovalCondition;

/// <summary>Var olan ApprovalCondition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalConditionCommand(Guid Id, UpdateApprovalConditionRequest Request)
    : IRequest<BaseResponse<bool>>;
