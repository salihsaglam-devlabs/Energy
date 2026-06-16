using Energy.Application.Modules.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepList;

/// <summary>
/// <see cref="GetApprovalRequestStepListQuery"/> handler'ı. <see cref="IApprovalRequestStepService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestStepListQueryHandler
    : IRequestHandler<GetApprovalRequestStepListQuery, BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>>
{
    private readonly IApprovalRequestStepService _service;

    public GetApprovalRequestStepListQueryHandler(IApprovalRequestStepService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>> Handle(
        GetApprovalRequestStepListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
