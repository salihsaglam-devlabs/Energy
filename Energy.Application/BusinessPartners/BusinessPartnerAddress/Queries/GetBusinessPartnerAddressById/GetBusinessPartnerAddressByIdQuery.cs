using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressById;

/// <summary>Kimliğe göre BusinessPartnerAddress detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBusinessPartnerAddressByIdQuery(Guid Id)
    : IRequest<BaseResponse<BusinessPartnerAddressDetailResponse>>;
