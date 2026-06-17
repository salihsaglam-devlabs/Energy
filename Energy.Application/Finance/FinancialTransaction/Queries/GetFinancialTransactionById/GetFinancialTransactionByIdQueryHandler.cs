using Energy.Application.Finance.FinancialTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Queries.GetFinancialTransactionById;

/// <summary>
/// <see cref="GetFinancialTransactionByIdQuery"/> handler'ı. <see cref="IFinancialTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialTransactionByIdQueryHandler
    : IRequestHandler<GetFinancialTransactionByIdQuery, BaseResponse<FinancialTransactionDetailResponse>>
{
    private readonly IFinancialTransactionService _service;

    public GetFinancialTransactionByIdQueryHandler(IFinancialTransactionService service)
        => _service = service;

    public Task<BaseResponse<FinancialTransactionDetailResponse>> Handle(
        GetFinancialTransactionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
