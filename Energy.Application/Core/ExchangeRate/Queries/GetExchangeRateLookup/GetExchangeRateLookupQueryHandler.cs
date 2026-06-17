using Energy.Application.Core.ExchangeRate.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Queries.GetExchangeRateLookup;

/// <summary>
/// <see cref="GetExchangeRateLookupQuery"/> handler'ı. <see cref="IExchangeRateLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetExchangeRateLookupQueryHandler
    : IRequestHandler<GetExchangeRateLookupQuery, BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>>
{
    private readonly IExchangeRateLookupService _lookup;

    public GetExchangeRateLookupQueryHandler(IExchangeRateLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>> Handle(
        GetExchangeRateLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
