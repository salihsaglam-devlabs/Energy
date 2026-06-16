using Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Commands.UpdateBusinessPartnerAddress;

/// <summary>
/// <see cref="UpdateBusinessPartnerAddressCommand"/> handler'ı. <see cref="IBusinessPartnerAddressService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBusinessPartnerAddressCommandHandler
    : IRequestHandler<UpdateBusinessPartnerAddressCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerAddressService _service;

    public UpdateBusinessPartnerAddressCommandHandler(IBusinessPartnerAddressService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBusinessPartnerAddressCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
