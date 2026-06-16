using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalAction.Commands.DeleteApprovalAction;

/// <summary>ApprovalAction kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalActionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
