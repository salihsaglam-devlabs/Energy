using Energy.Application.Modules.Projects.ProjectType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Queries.GetProjectTypeById;

/// <summary>
/// <see cref="GetProjectTypeByIdQuery"/> handler'ı. <see cref="IProjectTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectTypeByIdQueryHandler
    : IRequestHandler<GetProjectTypeByIdQuery, BaseResponse<ProjectTypeDetailResponse>>
{
    private readonly IProjectTypeService _service;

    public GetProjectTypeByIdQueryHandler(IProjectTypeService service)
        => _service = service;

    public Task<BaseResponse<ProjectTypeDetailResponse>> Handle(
        GetProjectTypeByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
