using Energy.Application.Finance.FinancialAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialAccount.Queries.GetFinancialAccountList;

/// <summary>
/// <see cref="GetFinancialAccountListQuery"/> handler'ı. <see cref="IFinancialAccountService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialAccountListQueryHandler
    : IRequestHandler<GetFinancialAccountListQuery, BaseResponse<PaginatedResponse<FinancialAccountListResponse>>>
{
    private readonly IFinancialAccountService _service;

    public GetFinancialAccountListQueryHandler(IFinancialAccountService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>> Handle(
        GetFinancialAccountListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
