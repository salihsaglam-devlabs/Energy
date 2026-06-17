using Energy.Application.BusinessPartners.BusinessPartner.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Commands.CreateBusinessPartner;

/// <summary>
/// <see cref="CreateBusinessPartnerCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBusinessPartnerService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBusinessPartnerCommandHandler
    : IRequestHandler<CreateBusinessPartnerCommand, BaseResponse<Guid>>
{
    private readonly IBusinessPartnerService _service;

    public CreateBusinessPartnerCommandHandler(IBusinessPartnerService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBusinessPartnerCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
