using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.DeletePermission;

public sealed class DeletePermissionCommandHandler
    : IRequestHandler<DeletePermissionCommand, BaseResponse<Guid>>
{
    private readonly IPermissionService _permissionService;

    public DeletePermissionCommandHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<BaseResponse<Guid>> Handle(
        DeletePermissionCommand request,
        CancellationToken cancellationToken)
    {
        await _permissionService.DeletePermissionAsync(request.Id, cancellationToken);
        return BaseResponse<Guid>.Success(request.Id);
    }
}
