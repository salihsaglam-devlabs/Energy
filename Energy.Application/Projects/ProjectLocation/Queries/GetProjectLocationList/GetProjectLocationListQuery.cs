using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectLocation.Queries.GetProjectLocationList;

/// <summary>Sayfalanmış ProjectLocation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectLocationListQuery(GetProjectLocationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectLocationListResponse>>>;
