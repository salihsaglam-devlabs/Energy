using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Commands.DeleteApprovalStepDefinition;

/// <summary>ApprovalStepDefinition kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalStepDefinitionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
