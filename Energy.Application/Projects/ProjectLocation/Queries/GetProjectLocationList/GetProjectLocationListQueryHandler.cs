using Energy.Application.Projects.ProjectLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectLocation.Queries.GetProjectLocationList;

/// <summary>
/// <see cref="GetProjectLocationListQuery"/> handler'ı. <see cref="IProjectLocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectLocationListQueryHandler
    : IRequestHandler<GetProjectLocationListQuery, BaseResponse<PaginatedResponse<ProjectLocationListResponse>>>
{
    private readonly IProjectLocationService _service;

    public GetProjectLocationListQueryHandler(IProjectLocationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectLocationListResponse>>> Handle(
        GetProjectLocationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
