using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, BaseResponse<Guid>>
{
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<Guid>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        await _roleService.DeleteRoleAsync(request.Id, cancellationToken);
        return BaseResponse<Guid>.Success(request.Id);
    }
}
