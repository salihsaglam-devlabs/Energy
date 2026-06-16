using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Commands.CreateApprovalAction;

/// <summary>Yeni ApprovalAction oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalActionCommand(CreateApprovalActionRequest Request)
    : IRequest<BaseResponse<Guid>>;
