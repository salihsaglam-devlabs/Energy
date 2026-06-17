using Energy.Application.BusinessPartners.BusinessPartner.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerLookup;

/// <summary>
/// <see cref="GetBusinessPartnerLookupQuery"/> handler'ı. <see cref="IBusinessPartnerLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerLookupQueryHandler
    : IRequestHandler<GetBusinessPartnerLookupQuery, BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>>
{
    private readonly IBusinessPartnerLookupService _lookup;

    public GetBusinessPartnerLookupQueryHandler(IBusinessPartnerLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>> Handle(
        GetBusinessPartnerLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
