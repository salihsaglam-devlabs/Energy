using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverList;

/// <summary>Sayfalanmış ApprovalStepApprover listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalStepApproverListQuery(GetApprovalStepApproverListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>>;
