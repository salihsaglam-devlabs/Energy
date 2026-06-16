using Energy.Application.Modules.Projects.Project.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Queries.GetProjectLookup;

/// <summary>
/// <see cref="GetProjectLookupQuery"/> handler'ı. <see cref="IProjectLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectLookupQueryHandler
    : IRequestHandler<GetProjectLookupQuery, BaseResponse<IReadOnlyList<ProjectLookupResponse>>>
{
    private readonly IProjectLookupService _lookup;

    public GetProjectLookupQueryHandler(IProjectLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectLookupResponse>>> Handle(
        GetProjectLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
