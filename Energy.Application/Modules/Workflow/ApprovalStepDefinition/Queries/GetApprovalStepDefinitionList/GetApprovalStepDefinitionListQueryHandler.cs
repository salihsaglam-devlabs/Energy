using Energy.Application.Modules.Workflow.ApprovalStepDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionList;

/// <summary>
/// <see cref="GetApprovalStepDefinitionListQuery"/> handler'ı. <see cref="IApprovalStepDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalStepDefinitionListQueryHandler
    : IRequestHandler<GetApprovalStepDefinitionListQuery, BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>>
{
    private readonly IApprovalStepDefinitionService _service;

    public GetApprovalStepDefinitionListQueryHandler(IApprovalStepDefinitionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>> Handle(
        GetApprovalStepDefinitionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
