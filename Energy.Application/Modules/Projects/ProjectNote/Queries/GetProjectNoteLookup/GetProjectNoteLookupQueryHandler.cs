using Energy.Application.Modules.Projects.ProjectNote.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectNote.Queries.GetProjectNoteLookup;

/// <summary>
/// <see cref="GetProjectNoteLookupQuery"/> handler'ı. <see cref="IProjectNoteLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectNoteLookupQueryHandler
    : IRequestHandler<GetProjectNoteLookupQuery, BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>>
{
    private readonly IProjectNoteLookupService _lookup;

    public GetProjectNoteLookupQueryHandler(IProjectNoteLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>> Handle(
        GetProjectNoteLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
