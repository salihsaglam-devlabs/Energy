using Energy.Application.Modules.Projects.ProjectLocation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Queries.GetProjectLocationLookup;

/// <summary>
/// <see cref="GetProjectLocationLookupQuery"/> handler'ı. <see cref="IProjectLocationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectLocationLookupQueryHandler
    : IRequestHandler<GetProjectLocationLookupQuery, BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>>
{
    private readonly IProjectLocationLookupService _lookup;

    public GetProjectLocationLookupQueryHandler(IProjectLocationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>> Handle(
        GetProjectLocationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
