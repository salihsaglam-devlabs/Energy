using Energy.Application.Finance.FinancialTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Queries.GetFinancialTransactionList;

/// <summary>
/// <see cref="GetFinancialTransactionListQuery"/> handler'ı. <see cref="IFinancialTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialTransactionListQueryHandler
    : IRequestHandler<GetFinancialTransactionListQuery, BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>>
{
    private readonly IFinancialTransactionService _service;

    public GetFinancialTransactionListQueryHandler(IFinancialTransactionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>> Handle(
        GetFinancialTransactionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
