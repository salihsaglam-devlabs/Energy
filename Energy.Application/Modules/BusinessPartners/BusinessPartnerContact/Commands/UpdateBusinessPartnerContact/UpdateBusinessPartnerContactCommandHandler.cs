using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Commands.UpdateBusinessPartnerContact;

/// <summary>
/// <see cref="UpdateBusinessPartnerContactCommand"/> handler'ı. <see cref="IBusinessPartnerContactService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBusinessPartnerContactCommandHandler
    : IRequestHandler<UpdateBusinessPartnerContactCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerContactService _service;

    public UpdateBusinessPartnerContactCommandHandler(IBusinessPartnerContactService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBusinessPartnerContactCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
