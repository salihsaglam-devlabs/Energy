using Energy.Application.Modules.Projects.ProjectLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Queries.GetProjectLocationById;

/// <summary>
/// <see cref="GetProjectLocationByIdQuery"/> handler'ı. <see cref="IProjectLocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectLocationByIdQueryHandler
    : IRequestHandler<GetProjectLocationByIdQuery, BaseResponse<ProjectLocationDetailResponse>>
{
    private readonly IProjectLocationService _service;

    public GetProjectLocationByIdQueryHandler(IProjectLocationService service)
        => _service = service;

    public Task<BaseResponse<ProjectLocationDetailResponse>> Handle(
        GetProjectLocationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
