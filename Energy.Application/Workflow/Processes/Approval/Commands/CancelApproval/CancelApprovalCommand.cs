using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;

namespace Energy.Application.Workflow.Processes.Approval.Commands.CancelApproval;

/// <summary>Onay talebini iptal eder use-case'i.</summary>
/// <param name="Id">Onay talebi kimliği.</param>
/// <param name="ActingUserId">İşlemi yapan kullanıcı kimliği.</param>
/// <param name="Note">Opsiyonel açıklama.</param>
public sealed record CancelApprovalCommand(Guid Id, Guid ActingUserId, string? Note)
    : IRequest<BaseResponse<ApprovalRequestListItemResponse>>;
