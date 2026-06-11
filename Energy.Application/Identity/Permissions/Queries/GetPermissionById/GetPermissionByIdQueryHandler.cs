using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Queries.GetPermissionById;

public sealed class GetPermissionByIdQueryHandler
    : IRequestHandler<GetPermissionByIdQuery, BaseResponse<PermissionResponse>>
{
    private readonly IPermissionService _permissionService;

    public GetPermissionByIdQueryHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<BaseResponse<PermissionResponse>> Handle(
        GetPermissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _permissionService.GetPermissionByIdAsync(request.Id, cancellationToken);
        return BaseResponse<PermissionResponse>.Success(result);
    }
}

