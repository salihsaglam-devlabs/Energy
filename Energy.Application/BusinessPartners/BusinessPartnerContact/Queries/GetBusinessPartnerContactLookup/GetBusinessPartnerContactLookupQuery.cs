using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactLookup;

/// <summary>BusinessPartnerContact lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBusinessPartnerContactLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>>;
