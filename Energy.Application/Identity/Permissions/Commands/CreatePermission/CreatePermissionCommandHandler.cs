using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.CreatePermission;

public sealed class CreatePermissionCommandHandler
    : IRequestHandler<CreatePermissionCommand, BaseResponse<PermissionResponse>>
{
    private readonly IPermissionService _permissionService;

    public CreatePermissionCommandHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<BaseResponse<PermissionResponse>> Handle(
        CreatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _permissionService.CreatePermissionAsync(request.Request, cancellationToken);
        return BaseResponse<PermissionResponse>.Success(result);
    }
}
