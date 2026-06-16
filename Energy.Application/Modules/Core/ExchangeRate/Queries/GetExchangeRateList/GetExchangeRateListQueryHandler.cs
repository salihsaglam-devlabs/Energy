using Energy.Application.Modules.Core.ExchangeRate.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.ExchangeRate.Queries.GetExchangeRateList;

/// <summary>
/// <see cref="GetExchangeRateListQuery"/> handler'ı. <see cref="IExchangeRateService"/>'i orkestre eder.
/// </summary>
public sealed class GetExchangeRateListQueryHandler
    : IRequestHandler<GetExchangeRateListQuery, BaseResponse<PaginatedResponse<ExchangeRateListResponse>>>
{
    private readonly IExchangeRateService _service;

    public GetExchangeRateListQueryHandler(IExchangeRateService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>> Handle(
        GetExchangeRateListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
