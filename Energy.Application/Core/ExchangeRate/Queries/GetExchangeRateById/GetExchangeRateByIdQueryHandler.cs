using Energy.Application.Core.ExchangeRate.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Queries.GetExchangeRateById;

/// <summary>
/// <see cref="GetExchangeRateByIdQuery"/> handler'ı. <see cref="IExchangeRateService"/>'i orkestre eder.
/// </summary>
public sealed class GetExchangeRateByIdQueryHandler
    : IRequestHandler<GetExchangeRateByIdQuery, BaseResponse<ExchangeRateDetailResponse>>
{
    private readonly IExchangeRateService _service;

    public GetExchangeRateByIdQueryHandler(IExchangeRateService service)
        => _service = service;

    public Task<BaseResponse<ExchangeRateDetailResponse>> Handle(
        GetExchangeRateByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
