using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionList;

/// <summary>Sayfalanmış ApprovalDefinition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalDefinitionListQuery(GetApprovalDefinitionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalDefinitionListResponse>>>;
