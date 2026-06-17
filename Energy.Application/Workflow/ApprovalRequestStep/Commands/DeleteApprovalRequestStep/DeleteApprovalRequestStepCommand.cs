using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestStep.Commands.DeleteApprovalRequestStep;

/// <summary>ApprovalRequestStep kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalRequestStepCommand(Guid Id) : IRequest<BaseResponse<bool>>;
