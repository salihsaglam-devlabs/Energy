using Energy.Application.Requests.Request.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Responses;
using MediatR;

namespace Energy.Application.Requests.Request.Queries.GetRequestList;

/// <summary>
/// <see cref="GetRequestListQuery"/> handler'ı. <see cref="IRequestService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestListQueryHandler
    : IRequestHandler<GetRequestListQuery, BaseResponse<PaginatedResponse<RequestListResponse>>>
{
    private readonly IRequestService _service;

    public GetRequestListQueryHandler(IRequestService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<RequestListResponse>>> Handle(
        GetRequestListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
