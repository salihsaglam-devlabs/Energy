using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenuPermissions;

public sealed class GetMenuPermissionsQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    public Guid MenuId { get; init; }

    public GetMenuPermissionsQuery(Guid menuId)
    {
        MenuId = menuId;
    }
}

