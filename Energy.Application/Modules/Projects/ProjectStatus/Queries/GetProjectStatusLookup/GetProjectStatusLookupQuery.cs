using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusLookup;

/// <summary>ProjectStatus lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProjectStatusLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>>;
