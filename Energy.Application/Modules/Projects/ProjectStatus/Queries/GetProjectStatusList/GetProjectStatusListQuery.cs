using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusList;

/// <summary>Sayfalanmış ProjectStatus listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectStatusListQuery(GetProjectStatusListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectStatusListResponse>>>;
