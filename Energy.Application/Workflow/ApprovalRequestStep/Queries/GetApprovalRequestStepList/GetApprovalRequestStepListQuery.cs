using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepList;

/// <summary>Sayfalanmış ApprovalRequestStep listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalRequestStepListQuery(GetApprovalRequestStepListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>>;
