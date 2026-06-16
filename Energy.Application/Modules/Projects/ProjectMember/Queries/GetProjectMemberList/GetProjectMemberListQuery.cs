using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectMember.Queries.GetProjectMemberList;

/// <summary>Sayfalanmış ProjectMember listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectMemberListQuery(GetProjectMemberListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectMemberListResponse>>>;
