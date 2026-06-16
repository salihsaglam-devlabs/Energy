using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Commands.CreateApprovalDefinitionVersion;

/// <summary>Yeni ApprovalDefinitionVersion oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalDefinitionVersionCommand(CreateApprovalDefinitionVersionRequest Request)
    : IRequest<BaseResponse<Guid>>;
