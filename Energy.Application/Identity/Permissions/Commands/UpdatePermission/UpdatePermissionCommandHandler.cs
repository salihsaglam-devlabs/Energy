using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.UpdatePermission;

public sealed class UpdatePermissionCommandHandler
    : IRequestHandler<UpdatePermissionCommand, BaseResponse<PermissionResponse>>
{
    private readonly IPermissionService _permissionService;

    public UpdatePermissionCommandHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<BaseResponse<PermissionResponse>> Handle(
        UpdatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _permissionService.UpdatePermissionAsync(request.Id, request.Request, cancellationToken);
        return BaseResponse<PermissionResponse>.Success(result);
    }
}
