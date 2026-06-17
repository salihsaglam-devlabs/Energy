using Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountLookup;

/// <summary>
/// <see cref="GetBusinessPartnerBankAccountLookupQuery"/> handler'ı. <see cref="IBusinessPartnerBankAccountLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerBankAccountLookupQueryHandler
    : IRequestHandler<GetBusinessPartnerBankAccountLookupQuery, BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>>
{
    private readonly IBusinessPartnerBankAccountLookupService _lookup;

    public GetBusinessPartnerBankAccountLookupQueryHandler(IBusinessPartnerBankAccountLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>> Handle(
        GetBusinessPartnerBankAccountLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
