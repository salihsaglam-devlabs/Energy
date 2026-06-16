using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerList;

/// <summary>Sayfalanmış BusinessPartner listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBusinessPartnerListQuery(GetBusinessPartnerListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>>;
