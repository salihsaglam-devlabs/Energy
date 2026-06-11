using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.SeedDefaultPermissions;

public sealed class SeedDefaultPermissionsCommandHandler
    : IRequestHandler<SeedDefaultPermissionsCommand, BaseResponse<SeedResultResponse>>
{
    private readonly IPermissionService _permissionService;

    public SeedDefaultPermissionsCommandHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<BaseResponse<SeedResultResponse>> Handle(
        SeedDefaultPermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _permissionService.SeedDefaultPermissionsAsync(cancellationToken);
        return BaseResponse<SeedResultResponse>.Success(result);
    }
}
