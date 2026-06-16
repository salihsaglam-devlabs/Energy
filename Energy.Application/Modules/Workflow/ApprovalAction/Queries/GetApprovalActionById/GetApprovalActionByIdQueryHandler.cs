using Energy.Application.Modules.Workflow.ApprovalAction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Queries.GetApprovalActionById;

/// <summary>
/// <see cref="GetApprovalActionByIdQuery"/> handler'ı. <see cref="IApprovalActionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalActionByIdQueryHandler
    : IRequestHandler<GetApprovalActionByIdQuery, BaseResponse<ApprovalActionDetailResponse>>
{
    private readonly IApprovalActionService _service;

    public GetApprovalActionByIdQueryHandler(IApprovalActionService service)
        => _service = service;

    public Task<BaseResponse<ApprovalActionDetailResponse>> Handle(
        GetApprovalActionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
