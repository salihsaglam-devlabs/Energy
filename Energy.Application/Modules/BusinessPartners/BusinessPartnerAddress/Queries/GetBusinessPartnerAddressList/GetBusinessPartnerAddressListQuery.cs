using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressList;

/// <summary>Sayfalanmış BusinessPartnerAddress listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBusinessPartnerAddressListQuery(GetBusinessPartnerAddressListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>>;
