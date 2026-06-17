using Energy.Application.BusinessPartners.BusinessPartnerAddress.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressList;

/// <summary>
/// <see cref="GetBusinessPartnerAddressListQuery"/> handler'ı. <see cref="IBusinessPartnerAddressService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerAddressListQueryHandler
    : IRequestHandler<GetBusinessPartnerAddressListQuery, BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>>
{
    private readonly IBusinessPartnerAddressService _service;

    public GetBusinessPartnerAddressListQueryHandler(IBusinessPartnerAddressService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>> Handle(
        GetBusinessPartnerAddressListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
