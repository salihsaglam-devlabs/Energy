using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Queries.GetLocalizationResourceLookup;

/// <summary>LocalizationResource lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetLocalizationResourceLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>>;
