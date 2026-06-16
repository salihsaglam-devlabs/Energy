using Energy.Application.Organization.LeaveRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Queries.GetLeaveRequestById;

/// <summary>
/// <see cref="GetLeaveRequestByIdQuery"/> handler'ı. <see cref="ILeaveRequestService"/>'i orkestre eder.
/// </summary>
public sealed class GetLeaveRequestByIdQueryHandler
    : IRequestHandler<GetLeaveRequestByIdQuery, BaseResponse<LeaveRequestDetailResponse>>
{
    private readonly ILeaveRequestService _service;

    public GetLeaveRequestByIdQueryHandler(ILeaveRequestService service)
        => _service = service;

    public Task<BaseResponse<LeaveRequestDetailResponse>> Handle(
        GetLeaveRequestByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
