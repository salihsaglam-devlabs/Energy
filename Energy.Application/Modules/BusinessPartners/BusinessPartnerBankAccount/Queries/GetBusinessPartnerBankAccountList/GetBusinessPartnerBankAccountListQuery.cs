using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountList;

/// <summary>Sayfalanmış BusinessPartnerBankAccount listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBusinessPartnerBankAccountListQuery(GetBusinessPartnerBankAccountListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>>;
