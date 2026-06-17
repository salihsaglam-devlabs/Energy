using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionList;

/// <summary>Sayfalanmış ApprovalDefinitionVersion listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetApprovalDefinitionVersionListQuery(GetApprovalDefinitionVersionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>>;
