using Energy.Application.Finance.FinancialTransactionLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Commands.CreateFinancialTransactionLine;

/// <summary>
/// <see cref="CreateFinancialTransactionLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IFinancialTransactionLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateFinancialTransactionLineCommandHandler
    : IRequestHandler<CreateFinancialTransactionLineCommand, BaseResponse<Guid>>
{
    private readonly IFinancialTransactionLineService _service;

    public CreateFinancialTransactionLineCommandHandler(IFinancialTransactionLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateFinancialTransactionLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
