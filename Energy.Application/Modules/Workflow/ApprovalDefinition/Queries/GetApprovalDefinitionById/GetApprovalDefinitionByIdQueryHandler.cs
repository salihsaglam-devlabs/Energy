using Energy.Application.Modules.Workflow.ApprovalDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionById;

/// <summary>
/// <see cref="GetApprovalDefinitionByIdQuery"/> handler'ı. <see cref="IApprovalDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDefinitionByIdQueryHandler
    : IRequestHandler<GetApprovalDefinitionByIdQuery, BaseResponse<ApprovalDefinitionDetailResponse>>
{
    private readonly IApprovalDefinitionService _service;

    public GetApprovalDefinitionByIdQueryHandler(IApprovalDefinitionService service)
        => _service = service;

    public Task<BaseResponse<ApprovalDefinitionDetailResponse>> Handle(
        GetApprovalDefinitionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
