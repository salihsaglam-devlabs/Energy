using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinition.Commands.UpdateApprovalDefinition;

/// <summary>Var olan ApprovalDefinition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateApprovalDefinitionCommand(Guid Id, UpdateApprovalDefinitionRequest Request)
    : IRequest<BaseResponse<bool>>;
