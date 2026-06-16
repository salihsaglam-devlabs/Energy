using Energy.Application.Projects.ProjectMember.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Commands.DeleteProjectMember;

/// <summary>
/// <see cref="DeleteProjectMemberCommand"/> handler'ı. <see cref="IProjectMemberService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectMemberCommandHandler
    : IRequestHandler<DeleteProjectMemberCommand, BaseResponse<bool>>
{
    private readonly IProjectMemberService _service;

    public DeleteProjectMemberCommandHandler(IProjectMemberService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectMemberCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
