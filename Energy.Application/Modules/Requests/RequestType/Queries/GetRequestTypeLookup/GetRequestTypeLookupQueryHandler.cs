using Energy.Application.Modules.Requests.RequestType.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeLookup;

/// <summary>
/// <see cref="GetRequestTypeLookupQuery"/> handler'ı. <see cref="IRequestTypeLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestTypeLookupQueryHandler
    : IRequestHandler<GetRequestTypeLookupQuery, BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>>
{
    private readonly IRequestTypeLookupService _lookup;

    public GetRequestTypeLookupQueryHandler(IRequestTypeLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>> Handle(
        GetRequestTypeLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
