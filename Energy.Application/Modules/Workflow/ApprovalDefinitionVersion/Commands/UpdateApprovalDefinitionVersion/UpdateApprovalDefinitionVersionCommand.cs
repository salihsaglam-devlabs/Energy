using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Commands.UpdateApprovalDefinitionVersion;

/// <summary>Var olan ApprovalDefinitionVersion kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalDefinitionVersionCommand(Guid Id, UpdateApprovalDefinitionVersionRequest Request)
    : IRequest<BaseResponse<bool>>;
