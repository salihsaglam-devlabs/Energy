using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationById;

/// <summary>Kimliğe göre ApprovalDelegation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalDelegationByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalDelegationDetailResponse>>;
