using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Commands.CreateApprovalRequest;

/// <summary>Yeni ApprovalRequest oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalRequestCommand(CreateApprovalRequestRequest Request)
    : IRequest<BaseResponse<Guid>>;
