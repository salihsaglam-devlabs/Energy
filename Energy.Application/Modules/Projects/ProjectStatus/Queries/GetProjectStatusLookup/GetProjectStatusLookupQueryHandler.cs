using Energy.Application.Modules.Projects.ProjectStatus.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusLookup;

/// <summary>
/// <see cref="GetProjectStatusLookupQuery"/> handler'ı. <see cref="IProjectStatusLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectStatusLookupQueryHandler
    : IRequestHandler<GetProjectStatusLookupQuery, BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>>
{
    private readonly IProjectStatusLookupService _lookup;

    public GetProjectStatusLookupQueryHandler(IProjectStatusLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>> Handle(
        GetProjectStatusLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
