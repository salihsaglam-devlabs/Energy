using Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Commands.CreateBusinessPartnerAddress;

/// <summary>
/// <see cref="CreateBusinessPartnerAddressCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBusinessPartnerAddressService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBusinessPartnerAddressCommandHandler
    : IRequestHandler<CreateBusinessPartnerAddressCommand, BaseResponse<Guid>>
{
    private readonly IBusinessPartnerAddressService _service;

    public CreateBusinessPartnerAddressCommandHandler(IBusinessPartnerAddressService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBusinessPartnerAddressCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
