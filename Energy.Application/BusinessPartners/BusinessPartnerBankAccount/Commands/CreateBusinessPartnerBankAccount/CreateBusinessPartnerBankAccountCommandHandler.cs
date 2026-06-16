using Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Commands.CreateBusinessPartnerBankAccount;

/// <summary>
/// <see cref="CreateBusinessPartnerBankAccountCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBusinessPartnerBankAccountService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBusinessPartnerBankAccountCommandHandler
    : IRequestHandler<CreateBusinessPartnerBankAccountCommand, BaseResponse<Guid>>
{
    private readonly IBusinessPartnerBankAccountService _service;

    public CreateBusinessPartnerBankAccountCommandHandler(IBusinessPartnerBankAccountService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBusinessPartnerBankAccountCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
