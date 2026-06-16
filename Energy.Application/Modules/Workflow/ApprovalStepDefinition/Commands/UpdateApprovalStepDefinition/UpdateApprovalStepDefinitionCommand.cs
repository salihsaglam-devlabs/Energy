using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Commands.UpdateApprovalStepDefinition;

/// <summary>Var olan ApprovalStepDefinition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalStepDefinitionCommand(Guid Id, UpdateApprovalStepDefinitionRequest Request)
    : IRequest<BaseResponse<bool>>;
