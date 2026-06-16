using Energy.Application.Workflow.ApprovalRequestApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverList;

/// <summary>
/// <see cref="GetApprovalRequestApproverListQuery"/> handler'ı. <see cref="IApprovalRequestApproverService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestApproverListQueryHandler
    : IRequestHandler<GetApprovalRequestApproverListQuery, BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>>
{
    private readonly IApprovalRequestApproverService _service;

    public GetApprovalRequestApproverListQueryHandler(IApprovalRequestApproverService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>> Handle(
        GetApprovalRequestApproverListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
