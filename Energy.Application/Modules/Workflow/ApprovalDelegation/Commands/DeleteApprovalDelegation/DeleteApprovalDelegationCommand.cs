using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.DeleteApprovalDelegation;

/// <summary>ApprovalDelegation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalDelegationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
