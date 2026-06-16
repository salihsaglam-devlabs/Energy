using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Queries.GetProjectPhasLookup;

/// <summary>ProjectPhas lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProjectPhasLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>>;
