using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalCondition.Commands.CreateApprovalCondition;

/// <summary>Yeni ApprovalCondition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalConditionCommand(CreateApprovalConditionRequest Request)
    : IRequest<BaseResponse<Guid>>;
