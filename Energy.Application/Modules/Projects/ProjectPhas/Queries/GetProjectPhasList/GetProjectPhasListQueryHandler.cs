using Energy.Application.Modules.Projects.ProjectPhas.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Queries.GetProjectPhasList;

/// <summary>
/// <see cref="GetProjectPhasListQuery"/> handler'ı. <see cref="IProjectPhasService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectPhasListQueryHandler
    : IRequestHandler<GetProjectPhasListQuery, BaseResponse<PaginatedResponse<ProjectPhasListResponse>>>
{
    private readonly IProjectPhasService _service;

    public GetProjectPhasListQueryHandler(IProjectPhasService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectPhasListResponse>>> Handle(
        GetProjectPhasListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
