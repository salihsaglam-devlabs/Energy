using Energy.Application.Modules.Finance.FinancialAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialAccount.Commands.CreateFinancialAccount;

/// <summary>
/// <see cref="CreateFinancialAccountCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IFinancialAccountService"/>'i orkestre eder.
/// </summary>
public sealed class CreateFinancialAccountCommandHandler
    : IRequestHandler<CreateFinancialAccountCommand, BaseResponse<Guid>>
{
    private readonly IFinancialAccountService _service;

    public CreateFinancialAccountCommandHandler(IFinancialAccountService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateFinancialAccountCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
