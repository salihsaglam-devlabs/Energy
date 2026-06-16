using Energy.Application.Modules.Core.Currency.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Currency.Queries.GetCurrencyLookup;

/// <summary>
/// <see cref="GetCurrencyLookupQuery"/> handler'ı. <see cref="ICurrencyLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetCurrencyLookupQueryHandler
    : IRequestHandler<GetCurrencyLookupQuery, BaseResponse<IReadOnlyList<CurrencyLookupResponse>>>
{
    private readonly ICurrencyLookupService _lookup;

    public GetCurrencyLookupQueryHandler(ICurrencyLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<CurrencyLookupResponse>>> Handle(
        GetCurrencyLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
