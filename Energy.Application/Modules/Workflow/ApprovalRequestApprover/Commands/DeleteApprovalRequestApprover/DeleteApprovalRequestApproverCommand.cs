using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.DeleteApprovalRequestApprover;

/// <summary>ApprovalRequestApprover kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalRequestApproverCommand(Guid Id) : IRequest<BaseResponse<bool>>;
