using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionList;

/// <summary>Sayfalanmış ApprovalStepDefinition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalStepDefinitionListQuery(GetApprovalStepDefinitionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>>;
