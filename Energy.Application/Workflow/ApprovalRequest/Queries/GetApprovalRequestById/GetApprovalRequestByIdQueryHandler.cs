using Energy.Application.Workflow.ApprovalRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Queries.GetApprovalRequestById;

/// <summary>
/// <see cref="GetApprovalRequestByIdQuery"/> handler'ı. <see cref="IApprovalRequestService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalRequestByIdQueryHandler
    : IRequestHandler<GetApprovalRequestByIdQuery, BaseResponse<ApprovalRequestDetailResponse>>
{
    private readonly IApprovalRequestService _service;

    public GetApprovalRequestByIdQueryHandler(IApprovalRequestService service)
        => _service = service;

    public Task<BaseResponse<ApprovalRequestDetailResponse>> Handle(
        GetApprovalRequestByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
