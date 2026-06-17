using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinition.Commands.CreateApprovalDefinition;

/// <summary>Yeni ApprovalDefinition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalDefinitionCommand(CreateApprovalDefinitionRequest Request)
    : IRequest<BaseResponse<Guid>>;
