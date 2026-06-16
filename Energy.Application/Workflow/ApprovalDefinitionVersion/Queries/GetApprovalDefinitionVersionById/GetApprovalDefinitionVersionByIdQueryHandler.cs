using Energy.Application.Workflow.ApprovalDefinitionVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionById;

/// <summary>
/// <see cref="GetApprovalDefinitionVersionByIdQuery"/> handler'ı. <see cref="IApprovalDefinitionVersionService"/>'i orkestre eder.
/// </summary>
public sealed class GetApprovalDefinitionVersionByIdQueryHandler
    : IRequestHandler<GetApprovalDefinitionVersionByIdQuery, BaseResponse<ApprovalDefinitionVersionDetailResponse>>
{
    private readonly IApprovalDefinitionVersionService _service;

    public GetApprovalDefinitionVersionByIdQueryHandler(IApprovalDefinitionVersionService service)
        => _service = service;

    public Task<BaseResponse<ApprovalDefinitionVersionDetailResponse>> Handle(
        GetApprovalDefinitionVersionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
