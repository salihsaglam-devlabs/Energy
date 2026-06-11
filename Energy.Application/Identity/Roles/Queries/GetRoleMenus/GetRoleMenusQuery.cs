using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRoleMenus;

public sealed class GetRoleMenusQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<MenuResponse>>>
{
    public Guid RoleId { get; init; }

    public GetRoleMenusQuery(Guid roleId)
    {
        RoleId = roleId;
    }
}
