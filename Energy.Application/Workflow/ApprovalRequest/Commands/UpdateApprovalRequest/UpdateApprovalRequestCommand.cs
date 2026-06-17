using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Commands.UpdateApprovalRequest;

/// <summary>Var olan ApprovalRequest kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalRequestCommand(Guid Id, UpdateApprovalRequestRequest Request)
    : IRequest<BaseResponse<bool>>;
