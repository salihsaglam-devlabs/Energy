using Energy.Application.BusinessPartners.BusinessPartner.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Commands.DeleteBusinessPartner;

/// <summary>
/// <see cref="DeleteBusinessPartnerCommand"/> handler'ı. <see cref="IBusinessPartnerService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBusinessPartnerCommandHandler
    : IRequestHandler<DeleteBusinessPartnerCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerService _service;

    public DeleteBusinessPartnerCommandHandler(IBusinessPartnerService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBusinessPartnerCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
