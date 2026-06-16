using Energy.Application.BusinessPartners.BusinessPartnerAddress.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressLookup;

/// <summary>
/// <see cref="GetBusinessPartnerAddressLookupQuery"/> handler'ı. <see cref="IBusinessPartnerAddressLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerAddressLookupQueryHandler
    : IRequestHandler<GetBusinessPartnerAddressLookupQuery, BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>>
{
    private readonly IBusinessPartnerAddressLookupService _lookup;

    public GetBusinessPartnerAddressLookupQueryHandler(IBusinessPartnerAddressLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>> Handle(
        GetBusinessPartnerAddressLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
