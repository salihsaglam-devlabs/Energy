using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Queries.GetProjectTypeLookup;

/// <summary>ProjectType lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProjectTypeLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>>;
