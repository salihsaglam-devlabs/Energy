using Energy.Application.Projects.ProjectType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectType.Queries.GetProjectTypeList;

/// <summary>
/// <see cref="GetProjectTypeListQuery"/> handler'ı. <see cref="IProjectTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectTypeListQueryHandler
    : IRequestHandler<GetProjectTypeListQuery, BaseResponse<PaginatedResponse<ProjectTypeListResponse>>>
{
    private readonly IProjectTypeService _service;

    public GetProjectTypeListQueryHandler(IProjectTypeService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectTypeListResponse>>> Handle(
        GetProjectTypeListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
