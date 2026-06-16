using Energy.Application.Modules.Projects.ProjectMember.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectMember.Commands.UpdateProjectMember;

/// <summary>
/// <see cref="UpdateProjectMemberCommand"/> handler'ı. <see cref="IProjectMemberService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectMemberCommandHandler
    : IRequestHandler<UpdateProjectMemberCommand, BaseResponse<bool>>
{
    private readonly IProjectMemberService _service;

    public UpdateProjectMemberCommandHandler(IProjectMemberService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectMemberCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
