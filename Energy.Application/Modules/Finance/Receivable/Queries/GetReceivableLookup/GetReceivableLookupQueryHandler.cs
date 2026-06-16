using Energy.Application.Modules.Finance.Receivable.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Receivable.Queries.GetReceivableLookup;

/// <summary>
/// <see cref="GetReceivableLookupQuery"/> handler'ı. <see cref="IReceivableLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetReceivableLookupQueryHandler
    : IRequestHandler<GetReceivableLookupQuery, BaseResponse<IReadOnlyList<ReceivableLookupResponse>>>
{
    private readonly IReceivableLookupService _lookup;

    public GetReceivableLookupQueryHandler(IReceivableLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>> Handle(
        GetReceivableLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
