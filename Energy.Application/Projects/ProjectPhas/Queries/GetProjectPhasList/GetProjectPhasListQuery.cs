using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectPhas.Queries.GetProjectPhasList;

/// <summary>Sayfalanmış ProjectPhas listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectPhasListQuery(GetProjectPhasListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectPhasListResponse>>>;
