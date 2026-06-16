using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationList;

/// <summary>Sayfalanmış ApprovalDelegation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalDelegationListQuery(GetApprovalDelegationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>>;
