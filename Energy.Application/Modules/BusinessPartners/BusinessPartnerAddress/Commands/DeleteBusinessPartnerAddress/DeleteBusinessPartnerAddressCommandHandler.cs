using Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Commands.DeleteBusinessPartnerAddress;

/// <summary>
/// <see cref="DeleteBusinessPartnerAddressCommand"/> handler'ı. <see cref="IBusinessPartnerAddressService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBusinessPartnerAddressCommandHandler
    : IRequestHandler<DeleteBusinessPartnerAddressCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerAddressService _service;

    public DeleteBusinessPartnerAddressCommandHandler(IBusinessPartnerAddressService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBusinessPartnerAddressCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
