using Energy.Application.Projects.ProjectMember.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Commands.CreateProjectMember;

/// <summary>
/// <see cref="CreateProjectMemberCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectMemberService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectMemberCommandHandler
    : IRequestHandler<CreateProjectMemberCommand, BaseResponse<Guid>>
{
    private readonly IProjectMemberService _service;

    public CreateProjectMemberCommandHandler(IProjectMemberService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectMemberCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
