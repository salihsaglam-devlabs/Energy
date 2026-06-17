using Energy.Application.Projects.ProjectMember.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberById;

/// <summary>
/// <see cref="GetProjectMemberByIdQuery"/> handler'ı. <see cref="IProjectMemberService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectMemberByIdQueryHandler
    : IRequestHandler<GetProjectMemberByIdQuery, BaseResponse<ProjectMemberDetailResponse>>
{
    private readonly IProjectMemberService _service;

    public GetProjectMemberByIdQueryHandler(IProjectMemberService service)
        => _service = service;

    public Task<BaseResponse<ProjectMemberDetailResponse>> Handle(
        GetProjectMemberByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
