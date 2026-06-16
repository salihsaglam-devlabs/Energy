using Energy.Application.Projects.ProjectMember.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberLookup;

/// <summary>
/// <see cref="GetProjectMemberLookupQuery"/> handler'ı. <see cref="IProjectMemberLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectMemberLookupQueryHandler
    : IRequestHandler<GetProjectMemberLookupQuery, BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>>
{
    private readonly IProjectMemberLookupService _lookup;

    public GetProjectMemberLookupQueryHandler(IProjectMemberLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>> Handle(
        GetProjectMemberLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
