using Energy.Application.Core.Currency.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Responses;
using MediatR;

namespace Energy.Application.Core.Currency.Queries.GetCurrencyById;

/// <summary>
/// <see cref="GetCurrencyByIdQuery"/> handler'ı. <see cref="ICurrencyService"/>'i orkestre eder.
/// </summary>
public sealed class GetCurrencyByIdQueryHandler
    : IRequestHandler<GetCurrencyByIdQuery, BaseResponse<CurrencyDetailResponse>>
{
    private readonly ICurrencyService _service;

    public GetCurrencyByIdQueryHandler(ICurrencyService service)
        => _service = service;

    public Task<BaseResponse<CurrencyDetailResponse>> Handle(
        GetCurrencyByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
