using Energy.Application.BusinessPartners.BusinessPartner.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Commands.UpdateBusinessPartner;

/// <summary>
/// <see cref="UpdateBusinessPartnerCommand"/> handler'ı. <see cref="IBusinessPartnerService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBusinessPartnerCommandHandler
    : IRequestHandler<UpdateBusinessPartnerCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerService _service;

    public UpdateBusinessPartnerCommandHandler(IBusinessPartnerService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBusinessPartnerCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
