using Energy.Application.Projects.ProjectMember.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberList;

/// <summary>
/// <see cref="GetProjectMemberListQuery"/> handler'ı. <see cref="IProjectMemberService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectMemberListQueryHandler
    : IRequestHandler<GetProjectMemberListQuery, BaseResponse<PaginatedResponse<ProjectMemberListResponse>>>
{
    private readonly IProjectMemberService _service;

    public GetProjectMemberListQueryHandler(IProjectMemberService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectMemberListResponse>>> Handle(
        GetProjectMemberListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
