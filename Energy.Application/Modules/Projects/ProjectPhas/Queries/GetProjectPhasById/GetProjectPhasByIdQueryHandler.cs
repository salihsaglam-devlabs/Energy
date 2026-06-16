using Energy.Application.Modules.Projects.ProjectPhas.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Queries.GetProjectPhasById;

/// <summary>
/// <see cref="GetProjectPhasByIdQuery"/> handler'ı. <see cref="IProjectPhasService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectPhasByIdQueryHandler
    : IRequestHandler<GetProjectPhasByIdQuery, BaseResponse<ProjectPhasDetailResponse>>
{
    private readonly IProjectPhasService _service;

    public GetProjectPhasByIdQueryHandler(IProjectPhasService service)
        => _service = service;

    public Task<BaseResponse<ProjectPhasDetailResponse>> Handle(
        GetProjectPhasByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
