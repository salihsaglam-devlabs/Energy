using Energy.Application.Requests.RequestLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Queries.GetRequestLineList;

/// <summary>
/// <see cref="GetRequestLineListQuery"/> handler'ı. <see cref="IRequestLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestLineListQueryHandler
    : IRequestHandler<GetRequestLineListQuery, BaseResponse<PaginatedResponse<RequestLineListResponse>>>
{
    private readonly IRequestLineService _service;

    public GetRequestLineListQueryHandler(IRequestLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<RequestLineListResponse>>> Handle(
        GetRequestLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
