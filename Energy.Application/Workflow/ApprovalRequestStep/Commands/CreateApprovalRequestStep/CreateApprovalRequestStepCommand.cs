using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestStep.Commands.CreateApprovalRequestStep;

/// <summary>Yeni ApprovalRequestStep oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalRequestStepCommand(CreateApprovalRequestStepRequest Request)
    : IRequest<BaseResponse<Guid>>;
