using Energy.Application.Workflow.ApprovalDefinitionVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionList;

/// <summary>
/// <see cref="GetApprovalDefinitionVersionListQuery"/> handler'ı. <see cref="IApprovalDefinitionVersionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDefinitionVersionListQueryHandler
    : IRequestHandler<GetApprovalDefinitionVersionListQuery, BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>>
{
    private readonly IApprovalDefinitionVersionService _service;

    public GetApprovalDefinitionVersionListQueryHandler(IApprovalDefinitionVersionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>> Handle(
        GetApprovalDefinitionVersionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
