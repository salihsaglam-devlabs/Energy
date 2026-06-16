using Energy.Application.Workflow.ApprovalStepApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverList;

/// <summary>
/// <see cref="GetApprovalStepApproverListQuery"/> handler'ı. <see cref="IApprovalStepApproverService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalStepApproverListQueryHandler
    : IRequestHandler<GetApprovalStepApproverListQuery, BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>>
{
    private readonly IApprovalStepApproverService _service;

    public GetApprovalStepApproverListQueryHandler(IApprovalStepApproverService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>> Handle(
        GetApprovalStepApproverListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
