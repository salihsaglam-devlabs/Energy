using Energy.Application.Workflow.ApprovalCondition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalCondition.Queries.GetApprovalConditionList;

/// <summary>
/// <see cref="GetApprovalConditionListQuery"/> handler'ı. <see cref="IApprovalConditionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalConditionListQueryHandler
    : IRequestHandler<GetApprovalConditionListQuery, BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>>
{
    private readonly IApprovalConditionService _service;

    public GetApprovalConditionListQueryHandler(IApprovalConditionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>> Handle(
        GetApprovalConditionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
