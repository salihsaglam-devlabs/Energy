using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Commands.CreateApprovalStepDefinition;

/// <summary>Yeni ApprovalStepDefinition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalStepDefinitionCommand(CreateApprovalStepDefinitionRequest Request)
    : IRequest<BaseResponse<Guid>>;
