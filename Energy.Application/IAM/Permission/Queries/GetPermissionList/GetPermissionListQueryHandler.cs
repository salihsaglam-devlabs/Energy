using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.Permission.Queries.GetPermissionList;

/// <summary><see cref="GetPermissionListQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetPermissionListQueryHandler
    : IRequestHandler<GetPermissionListQuery, BaseResponse<IReadOnlyList<PermissionResponse>>>
{
    private readonly IPermissionService _permissions;

    public GetPermissionListQueryHandler(IPermissionService permissions)
    {
        _permissions = permissions;
    }

    public async Task<BaseResponse<IReadOnlyList<PermissionResponse>>> Handle(GetPermissionListQuery request, CancellationToken ct)
    {
        var result = await _permissions.GetAllAsync(ct);
        return BaseResponse<IReadOnlyList<PermissionResponse>>.Success(result);
    }
}
