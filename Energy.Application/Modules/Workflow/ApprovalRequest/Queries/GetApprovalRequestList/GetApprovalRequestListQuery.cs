using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Queries.GetApprovalRequestList;

/// <summary>Sayfalanmış ApprovalRequest listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalRequestListQuery(GetApprovalRequestListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>>;
