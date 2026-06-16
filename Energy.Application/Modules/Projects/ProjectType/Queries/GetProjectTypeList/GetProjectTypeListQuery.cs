using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Queries.GetProjectTypeList;

/// <summary>Sayfalanmış ProjectType listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectTypeListQuery(GetProjectTypeListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectTypeListResponse>>>;
