using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Commands.DeleteApprovalCondition;

/// <summary>ApprovalCondition kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalConditionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
