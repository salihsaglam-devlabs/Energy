using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRolePermissions;

public sealed class GetRolePermissionsQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    public Guid RoleId { get; init; }

    public GetRolePermissionsQuery(Guid roleId)
    {
        RoleId = roleId;
    }
}

