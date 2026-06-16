using Energy.Application.Projects.ProjectType.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectType.Queries.GetProjectTypeLookup;

/// <summary>
/// <see cref="GetProjectTypeLookupQuery"/> handler'ı. <see cref="IProjectTypeLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectTypeLookupQueryHandler
    : IRequestHandler<GetProjectTypeLookupQuery, BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>>
{
    private readonly IProjectTypeLookupService _lookup;

    public GetProjectTypeLookupQueryHandler(IProjectTypeLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>> Handle(
        GetProjectTypeLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
