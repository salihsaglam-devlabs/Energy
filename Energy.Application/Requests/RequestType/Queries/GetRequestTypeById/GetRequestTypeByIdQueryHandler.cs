using Energy.Application.Requests.RequestType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestType.Queries.GetRequestTypeById;

/// <summary>
/// <see cref="GetRequestTypeByIdQuery"/> handler'ı. <see cref="IRequestTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestTypeByIdQueryHandler
    : IRequestHandler<GetRequestTypeByIdQuery, BaseResponse<RequestTypeDetailResponse>>
{
    private readonly IRequestTypeService _service;

    public GetRequestTypeByIdQueryHandler(IRequestTypeService service)
        => _service = service;

    public Task<BaseResponse<RequestTypeDetailResponse>> Handle(
        GetRequestTypeByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
