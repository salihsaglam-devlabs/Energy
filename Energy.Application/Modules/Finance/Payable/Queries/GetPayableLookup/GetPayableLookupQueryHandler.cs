using Energy.Application.Modules.Finance.Payable.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Queries.GetPayableLookup;

/// <summary>
/// <see cref="GetPayableLookupQuery"/> handler'ı. <see cref="IPayableLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPayableLookupQueryHandler
    : IRequestHandler<GetPayableLookupQuery, BaseResponse<IReadOnlyList<PayableLookupResponse>>>
{
    private readonly IPayableLookupService _lookup;

    public GetPayableLookupQueryHandler(IPayableLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PayableLookupResponse>>> Handle(
        GetPayableLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
