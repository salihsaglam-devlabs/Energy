using Energy.Application.Modules.Workflow.ApprovalRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Queries.GetApprovalRequestList;

/// <summary>
/// <see cref="GetApprovalRequestListQuery"/> handler'ı. <see cref="IApprovalRequestService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestListQueryHandler
    : IRequestHandler<GetApprovalRequestListQuery, BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>>
{
    private readonly IApprovalRequestService _service;

    public GetApprovalRequestListQueryHandler(IApprovalRequestService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>> Handle(
        GetApprovalRequestListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
