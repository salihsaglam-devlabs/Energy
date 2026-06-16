using Energy.Application.Modules.Requests.Request.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.Request.Queries.GetRequestLookup;

/// <summary>
/// <see cref="GetRequestLookupQuery"/> handler'ı. <see cref="IRequestLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestLookupQueryHandler
    : IRequestHandler<GetRequestLookupQuery, BaseResponse<IReadOnlyList<RequestLookupResponse>>>
{
    private readonly IRequestLookupService _lookup;

    public GetRequestLookupQueryHandler(IRequestLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<RequestLookupResponse>>> Handle(
        GetRequestLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
