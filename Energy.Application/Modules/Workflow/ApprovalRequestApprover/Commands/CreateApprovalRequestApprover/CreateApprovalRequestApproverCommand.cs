using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.CreateApprovalRequestApprover;

/// <summary>Yeni ApprovalRequestApprover oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalRequestApproverCommand(CreateApprovalRequestApproverRequest Request)
    : IRequest<BaseResponse<Guid>>;
