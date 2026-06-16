using Energy.Application.Modules.Organization.LeaveRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.LeaveRequest.Queries.GetLeaveRequestList;

/// <summary>
/// <see cref="GetLeaveRequestListQuery"/> handler'ı. <see cref="ILeaveRequestService"/>'i orkestre eder.
/// </summary>
public sealed class GetLeaveRequestListQueryHandler
    : IRequestHandler<GetLeaveRequestListQuery, BaseResponse<PaginatedResponse<LeaveRequestListResponse>>>
{
    private readonly ILeaveRequestService _service;

    public GetLeaveRequestListQueryHandler(ILeaveRequestService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>> Handle(
        GetLeaveRequestListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
