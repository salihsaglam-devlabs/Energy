using Energy.Application.Requests.RequestLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Queries.GetRequestLineLookup;

/// <summary>
/// <see cref="GetRequestLineLookupQuery"/> handler'ı. <see cref="IRequestLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestLineLookupQueryHandler
    : IRequestHandler<GetRequestLineLookupQuery, BaseResponse<IReadOnlyList<RequestLineLookupResponse>>>
{
    private readonly IRequestLineLookupService _lookup;

    public GetRequestLineLookupQueryHandler(IRequestLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<RequestLineLookupResponse>>> Handle(
        GetRequestLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
