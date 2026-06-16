using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Role.Commands.DeleteRole;

/// <summary><see cref="DeleteRoleCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, BaseResponse<bool>>
{
    private readonly IRoleService _roles;

    public DeleteRoleCommandHandler(IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteRoleCommand request, CancellationToken ct)
    {
        var result = await _roles.DeleteAsync(request.Id, ct);
        return BaseResponse<bool>.Success(result);
    }
}
