using Energy.Application.Workflow.ApprovalStepApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverById;

/// <summary>
/// <see cref="GetApprovalStepApproverByIdQuery"/> handler'ı. <see cref="IApprovalStepApproverService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalStepApproverByIdQueryHandler
    : IRequestHandler<GetApprovalStepApproverByIdQuery, BaseResponse<ApprovalStepApproverDetailResponse>>
{
    private readonly IApprovalStepApproverService _service;

    public GetApprovalStepApproverByIdQueryHandler(IApprovalStepApproverService service)
        => _service = service;

    public Task<BaseResponse<ApprovalStepApproverDetailResponse>> Handle(
        GetApprovalStepApproverByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
