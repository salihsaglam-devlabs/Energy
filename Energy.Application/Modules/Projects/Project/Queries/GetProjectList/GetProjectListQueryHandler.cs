using Energy.Application.Modules.Projects.Project.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Queries.GetProjectList;

/// <summary>
/// <see cref="GetProjectListQuery"/> handler'ı. <see cref="IProjectService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectListQueryHandler
    : IRequestHandler<GetProjectListQuery, BaseResponse<PaginatedResponse<ProjectListResponse>>>
{
    private readonly IProjectService _service;

    public GetProjectListQueryHandler(IProjectService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectListResponse>>> Handle(
        GetProjectListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
