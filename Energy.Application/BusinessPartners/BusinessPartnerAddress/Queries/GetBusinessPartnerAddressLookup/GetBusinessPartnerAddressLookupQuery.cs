using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressLookup;

/// <summary>BusinessPartnerAddress lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBusinessPartnerAddressLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>>;
