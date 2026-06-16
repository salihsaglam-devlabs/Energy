using Energy.Application.Projects.ProjectStatus.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectStatus.Queries.GetProjectStatusList;

/// <summary>
/// <see cref="GetProjectStatusListQuery"/> handler'ı. <see cref="IProjectStatusService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectStatusListQueryHandler
    : IRequestHandler<GetProjectStatusListQuery, BaseResponse<PaginatedResponse<ProjectStatusListResponse>>>
{
    private readonly IProjectStatusService _service;

    public GetProjectStatusListQueryHandler(IProjectStatusService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectStatusListResponse>>> Handle(
        GetProjectStatusListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
