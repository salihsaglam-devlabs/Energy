using Energy.Application.Workflow.ApprovalStepDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionById;

/// <summary>
/// <see cref="GetApprovalStepDefinitionByIdQuery"/> handler'ı. <see cref="IApprovalStepDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalStepDefinitionByIdQueryHandler
    : IRequestHandler<GetApprovalStepDefinitionByIdQuery, BaseResponse<ApprovalStepDefinitionDetailResponse>>
{
    private readonly IApprovalStepDefinitionService _service;

    public GetApprovalStepDefinitionByIdQueryHandler(IApprovalStepDefinitionService service)
        => _service = service;

    public Task<BaseResponse<ApprovalStepDefinitionDetailResponse>> Handle(
        GetApprovalStepDefinitionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
