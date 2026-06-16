using Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Commands.UpdateBusinessPartnerBankAccount;

/// <summary>
/// <see cref="UpdateBusinessPartnerBankAccountCommand"/> handler'ı. <see cref="IBusinessPartnerBankAccountService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBusinessPartnerBankAccountCommandHandler
    : IRequestHandler<UpdateBusinessPartnerBankAccountCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerBankAccountService _service;

    public UpdateBusinessPartnerBankAccountCommandHandler(IBusinessPartnerBankAccountService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBusinessPartnerBankAccountCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
