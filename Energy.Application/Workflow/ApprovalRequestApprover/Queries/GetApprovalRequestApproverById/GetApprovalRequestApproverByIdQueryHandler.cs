using Energy.Application.Workflow.ApprovalRequestApprover.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverById;

/// <summary>
/// <see cref="GetApprovalRequestApproverByIdQuery"/> handler'ı. <see cref="IApprovalRequestApproverService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestApproverByIdQueryHandler
    : IRequestHandler<GetApprovalRequestApproverByIdQuery, BaseResponse<ApprovalRequestApproverDetailResponse>>
{
    private readonly IApprovalRequestApproverService _service;

    public GetApprovalRequestApproverByIdQueryHandler(IApprovalRequestApproverService service)
        => _service = service;

    public Task<BaseResponse<ApprovalRequestApproverDetailResponse>> Handle(
        GetApprovalRequestApproverByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
