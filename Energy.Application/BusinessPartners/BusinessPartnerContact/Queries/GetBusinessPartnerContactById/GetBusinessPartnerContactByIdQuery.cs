using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactById;

/// <summary>Kimliğe göre BusinessPartnerContact detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBusinessPartnerContactByIdQuery(Guid Id)
    : IRequest<BaseResponse<BusinessPartnerContactDetailResponse>>;
