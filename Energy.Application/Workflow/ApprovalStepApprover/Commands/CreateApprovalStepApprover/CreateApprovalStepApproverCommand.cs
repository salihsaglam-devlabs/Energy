using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Commands.CreateApprovalStepApprover;

/// <summary>Yeni ApprovalStepApprover oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalStepApproverCommand(CreateApprovalStepApproverRequest Request)
    : IRequest<BaseResponse<Guid>>;
