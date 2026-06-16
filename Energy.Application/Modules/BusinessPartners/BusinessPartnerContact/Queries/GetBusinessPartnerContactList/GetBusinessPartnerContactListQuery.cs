using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactList;

/// <summary>Sayfalanmış BusinessPartnerContact listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBusinessPartnerContactListQuery(GetBusinessPartnerContactListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>>;
