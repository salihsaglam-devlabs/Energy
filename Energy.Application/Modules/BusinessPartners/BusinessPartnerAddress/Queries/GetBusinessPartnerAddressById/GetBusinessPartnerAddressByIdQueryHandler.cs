using Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressById;

/// <summary>
/// <see cref="GetBusinessPartnerAddressByIdQuery"/> handler'ı. <see cref="IBusinessPartnerAddressService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerAddressByIdQueryHandler
    : IRequestHandler<GetBusinessPartnerAddressByIdQuery, BaseResponse<BusinessPartnerAddressDetailResponse>>
{
    private readonly IBusinessPartnerAddressService _service;

    public GetBusinessPartnerAddressByIdQueryHandler(IBusinessPartnerAddressService service)
        => _service = service;

    public Task<BaseResponse<BusinessPartnerAddressDetailResponse>> Handle(
        GetBusinessPartnerAddressByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
