using Energy.Application.Projects.Project.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Responses;
using MediatR;

namespace Energy.Application.Projects.Project.Queries.GetProjectById;

/// <summary>
/// <see cref="GetProjectByIdQuery"/> handler'ı. <see cref="IProjectService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, BaseResponse<ProjectDetailResponse>>
{
    private readonly IProjectService _service;

    public GetProjectByIdQueryHandler(IProjectService service)
        => _service = service;

    public Task<BaseResponse<ProjectDetailResponse>> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
