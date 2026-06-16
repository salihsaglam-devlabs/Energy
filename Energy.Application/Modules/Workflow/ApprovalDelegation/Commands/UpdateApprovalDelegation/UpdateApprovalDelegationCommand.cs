using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.UpdateApprovalDelegation;

/// <summary>Var olan ApprovalDelegation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalDelegationCommand(Guid Id, UpdateApprovalDelegationRequest Request)
    : IRequest<BaseResponse<bool>>;
