using Energy.Application.Modules.Projects.ProjectPhas.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Queries.GetProjectPhasLookup;

/// <summary>
/// <see cref="GetProjectPhasLookupQuery"/> handler'ı. <see cref="IProjectPhasLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectPhasLookupQueryHandler
    : IRequestHandler<GetProjectPhasLookupQuery, BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>>
{
    private readonly IProjectPhasLookupService _lookup;

    public GetProjectPhasLookupQueryHandler(IProjectPhasLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>> Handle(
        GetProjectPhasLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
