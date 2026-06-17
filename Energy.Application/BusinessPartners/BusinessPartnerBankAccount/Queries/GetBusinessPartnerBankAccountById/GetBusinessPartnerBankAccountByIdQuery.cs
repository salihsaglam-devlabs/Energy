using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountById;

/// <summary>Kimliğe göre BusinessPartnerBankAccount detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBusinessPartnerBankAccountByIdQuery(Guid Id)
    : IRequest<BaseResponse<BusinessPartnerBankAccountDetailResponse>>;
