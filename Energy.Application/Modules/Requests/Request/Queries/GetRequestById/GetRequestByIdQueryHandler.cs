using Energy.Application.Modules.Requests.Request.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.Request.Queries.GetRequestById;

/// <summary>
/// <see cref="GetRequestByIdQuery"/> handler'ı. <see cref="IRequestService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestByIdQueryHandler
    : IRequestHandler<GetRequestByIdQuery, BaseResponse<RequestDetailResponse>>
{
    private readonly IRequestService _service;

    public GetRequestByIdQueryHandler(IRequestService service)
        => _service = service;

    public Task<BaseResponse<RequestDetailResponse>> Handle(
        GetRequestByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
