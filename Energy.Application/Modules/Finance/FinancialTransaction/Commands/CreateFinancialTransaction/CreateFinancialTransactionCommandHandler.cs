using Energy.Application.Modules.Finance.FinancialTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransaction.Commands.CreateFinancialTransaction;

/// <summary>
/// <see cref="CreateFinancialTransactionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IFinancialTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateFinancialTransactionCommandHandler
    : IRequestHandler<CreateFinancialTransactionCommand, BaseResponse<Guid>>
{
    private readonly IFinancialTransactionService _service;

    public CreateFinancialTransactionCommandHandler(IFinancialTransactionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateFinancialTransactionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
