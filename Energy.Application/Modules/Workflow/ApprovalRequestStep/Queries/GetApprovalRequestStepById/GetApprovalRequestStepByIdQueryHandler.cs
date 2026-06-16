using Energy.Application.Modules.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepById;

/// <summary>
/// <see cref="GetApprovalRequestStepByIdQuery"/> handler'ı. <see cref="IApprovalRequestStepService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestStepByIdQueryHandler
    : IRequestHandler<GetApprovalRequestStepByIdQuery, BaseResponse<ApprovalRequestStepDetailResponse>>
{
    private readonly IApprovalRequestStepService _service;

    public GetApprovalRequestStepByIdQueryHandler(IApprovalRequestStepService service)
        => _service = service;

    public Task<BaseResponse<ApprovalRequestStepDetailResponse>> Handle(
        GetApprovalRequestStepByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
