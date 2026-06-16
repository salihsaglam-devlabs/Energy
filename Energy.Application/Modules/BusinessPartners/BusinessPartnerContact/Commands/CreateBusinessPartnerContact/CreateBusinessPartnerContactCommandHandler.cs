using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Commands.CreateBusinessPartnerContact;

/// <summary>
/// <see cref="CreateBusinessPartnerContactCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBusinessPartnerContactService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBusinessPartnerContactCommandHandler
    : IRequestHandler<CreateBusinessPartnerContactCommand, BaseResponse<Guid>>
{
    private readonly IBusinessPartnerContactService _service;

    public CreateBusinessPartnerContactCommandHandler(IBusinessPartnerContactService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBusinessPartnerContactCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
