using Energy.Application.Modules.Workflow.ApprovalDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionList;

/// <summary>
/// <see cref="GetApprovalDefinitionListQuery"/> handler'ı. <see cref="IApprovalDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDefinitionListQueryHandler
    : IRequestHandler<GetApprovalDefinitionListQuery, BaseResponse<PaginatedResponse<ApprovalDefinitionListResponse>>>
{
    private readonly IApprovalDefinitionService _service;

    public GetApprovalDefinitionListQueryHandler(IApprovalDefinitionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalDefinitionListResponse>>> Handle(
        GetApprovalDefinitionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
