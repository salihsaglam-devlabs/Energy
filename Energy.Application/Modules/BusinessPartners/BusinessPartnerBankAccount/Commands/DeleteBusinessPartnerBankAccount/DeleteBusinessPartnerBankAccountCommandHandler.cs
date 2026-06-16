using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Commands.DeleteBusinessPartnerBankAccount;

/// <summary>
/// <see cref="DeleteBusinessPartnerBankAccountCommand"/> handler'ı. <see cref="IBusinessPartnerBankAccountService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBusinessPartnerBankAccountCommandHandler
    : IRequestHandler<DeleteBusinessPartnerBankAccountCommand, BaseResponse<bool>>
{
    private readonly IBusinessPartnerBankAccountService _service;

    public DeleteBusinessPartnerBankAccountCommandHandler(IBusinessPartnerBankAccountService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBusinessPartnerBankAccountCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
