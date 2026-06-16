using Energy.Application.Modules.Projects.ProjectStatus.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusById;

/// <summary>
/// <see cref="GetProjectStatusByIdQuery"/> handler'ı. <see cref="IProjectStatusService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectStatusByIdQueryHandler
    : IRequestHandler<GetProjectStatusByIdQuery, BaseResponse<ProjectStatusDetailResponse>>
{
    private readonly IProjectStatusService _service;

    public GetProjectStatusByIdQueryHandler(IProjectStatusService service)
        => _service = service;

    public Task<BaseResponse<ProjectStatusDetailResponse>> Handle(
        GetProjectStatusByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
