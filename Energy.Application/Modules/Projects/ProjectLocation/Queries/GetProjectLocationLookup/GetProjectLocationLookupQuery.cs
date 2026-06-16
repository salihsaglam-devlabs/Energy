using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Queries.GetProjectLocationLookup;

/// <summary>ProjectLocation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProjectLocationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>>;
