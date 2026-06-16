using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectNote.Queries.GetProjectNoteLookup;

/// <summary>ProjectNote lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProjectNoteLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>>;
