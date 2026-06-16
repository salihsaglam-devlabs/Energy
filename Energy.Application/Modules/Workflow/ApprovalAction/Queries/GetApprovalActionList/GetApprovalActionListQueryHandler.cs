using Energy.Application.Modules.Workflow.ApprovalAction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Queries.GetApprovalActionList;

/// <summary>
/// <see cref="GetApprovalActionListQuery"/> handler'ı. <see cref="IApprovalActionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalActionListQueryHandler
    : IRequestHandler<GetApprovalActionListQuery, BaseResponse<PaginatedResponse<ApprovalActionListResponse>>>
{
    private readonly IApprovalActionService _service;

    public GetApprovalActionListQueryHandler(IApprovalActionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalActionListResponse>>> Handle(
        GetApprovalActionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
