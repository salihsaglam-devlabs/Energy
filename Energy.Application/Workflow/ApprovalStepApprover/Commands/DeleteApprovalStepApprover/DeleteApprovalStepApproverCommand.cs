using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Commands.DeleteApprovalStepApprover;

/// <summary>ApprovalStepApprover kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalStepApproverCommand(Guid Id) : IRequest<BaseResponse<bool>>;
