using Energy.Application.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Commands.DeleteBusinessPartnerContact;

/// <summary>
/// <see cref="DeleteBusinessPartnerContactCommand"/> handler'ı. <see cref="IBusinessPartnerContactService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBusinessPartnerContactCommandHandler
    : IRequestHandler<DeleteBusinessPartnerContactCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerContactService _service;

    public DeleteBusinessPartnerContactCommandHandler(IBusinessPartnerContactService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBusinessPartnerContactCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
