using Energy.Application.Modules.Core.Currency.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Currency.Queries.GetCurrencyList;

/// <summary>
/// <see cref="GetCurrencyListQuery"/> handler'ı. <see cref="ICurrencyService"/>'i orkestre eder.
/// </summary>
public sealed class GetCurrencyListQueryHandler
    : IRequestHandler<GetCurrencyListQuery, BaseResponse<PaginatedResponse<CurrencyListResponse>>>
{
    private readonly ICurrencyService _service;

    public GetCurrencyListQueryHandler(ICurrencyService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<CurrencyListResponse>>> Handle(
        GetCurrencyListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
