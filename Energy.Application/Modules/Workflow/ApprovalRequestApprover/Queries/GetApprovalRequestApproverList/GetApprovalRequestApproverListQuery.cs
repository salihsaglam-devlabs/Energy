using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverList;

/// <summary>Sayfalanmış ApprovalRequestApprover listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalRequestApproverListQuery(GetApprovalRequestApproverListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>>;
