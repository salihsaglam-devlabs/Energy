using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerById;

/// <summary>Kimliğe göre BusinessPartner detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBusinessPartnerByIdQuery(Guid Id)
    : IRequest<BaseResponse<BusinessPartnerDetailResponse>>;
