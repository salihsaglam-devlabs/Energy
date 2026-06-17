using Energy.Application.Requests.RequestLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Queries.GetRequestLineById;

/// <summary>
/// <see cref="GetRequestLineByIdQuery"/> handler'ı. <see cref="IRequestLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestLineByIdQueryHandler
    : IRequestHandler<GetRequestLineByIdQuery, BaseResponse<RequestLineDetailResponse>>
{
    private readonly IRequestLineService _service;

    public GetRequestLineByIdQueryHandler(IRequestLineService service)
        => _service = service;

    public Task<BaseResponse<RequestLineDetailResponse>> Handle(
        GetRequestLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
