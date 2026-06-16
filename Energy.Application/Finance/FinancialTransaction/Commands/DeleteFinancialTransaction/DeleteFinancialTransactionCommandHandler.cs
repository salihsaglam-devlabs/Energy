using Energy.Application.Finance.FinancialTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Commands.DeleteFinancialTransaction;

/// <summary>
/// <see cref="DeleteFinancialTransactionCommand"/> handler'ı. <see cref="IFinancialTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteFinancialTransactionCommandHandler
    : IRequestHandler<DeleteFinancialTransactionCommand, BaseResponse<bool>>
{
    private readonly IFinancialTransactionService _service;

    public DeleteFinancialTransactionCommandHandler(IFinancialTransactionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteFinancialTransactionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
