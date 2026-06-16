using Energy.Application.Modules.Workflow.ApprovalCondition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Queries.GetApprovalConditionById;

/// <summary>
/// <see cref="GetApprovalConditionByIdQuery"/> handler'ı. <see cref="IApprovalConditionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalConditionByIdQueryHandler
    : IRequestHandler<GetApprovalConditionByIdQuery, BaseResponse<ApprovalConditionDetailResponse>>
{
    private readonly IApprovalConditionService _service;

    public GetApprovalConditionByIdQueryHandler(IApprovalConditionService service)
        => _service = service;

    public Task<BaseResponse<ApprovalConditionDetailResponse>> Handle(
        GetApprovalConditionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
