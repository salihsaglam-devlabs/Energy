using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.CreateApprovalDelegation;

/// <summary>Yeni ApprovalDelegation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateApprovalDelegationCommand(CreateApprovalDelegationRequest Request)
    : IRequest<BaseResponse<Guid>>;
