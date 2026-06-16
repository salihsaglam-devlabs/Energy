using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerLookup;

/// <summary>BusinessPartner lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBusinessPartnerLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>>;
