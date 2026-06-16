using Energy.Application.BusinessPartners.BusinessPartnerContact.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactLookup;

/// <summary>
/// <see cref="GetBusinessPartnerContactLookupQuery"/> handler'ı. <see cref="IBusinessPartnerContactLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerContactLookupQueryHandler
    : IRequestHandler<GetBusinessPartnerContactLookupQuery, BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>>
{
    private readonly IBusinessPartnerContactLookupService _lookup;

    public GetBusinessPartnerContactLookupQueryHandler(IBusinessPartnerContactLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>> Handle(
        GetBusinessPartnerContactLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
