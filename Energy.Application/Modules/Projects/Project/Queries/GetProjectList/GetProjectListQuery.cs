using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Requests;
using Energy.Shared.Models.V1.Projects.Project.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Queries.GetProjectList;

/// <summary>Sayfalanmış Project listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectListQuery(GetProjectListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectListResponse>>>;
