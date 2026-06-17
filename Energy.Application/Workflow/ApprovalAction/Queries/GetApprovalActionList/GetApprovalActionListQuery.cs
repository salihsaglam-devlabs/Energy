using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalAction.Queries.GetApprovalActionList;

/// <summary>Sayfalanmış ApprovalAction listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalActionListQuery(GetApprovalActionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalActionListResponse>>>;
