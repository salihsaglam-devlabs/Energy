using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Queries.GetApprovalConditionList;

/// <summary>Sayfalanmış ApprovalCondition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalConditionListQuery(GetApprovalConditionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>>;
