using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.SetRoleMenus;

public sealed record SetRoleMenusCommand(Guid RoleId, IReadOnlyCollection<Guid> MenuIds)
    : IRequest<BaseResponse<IReadOnlyList<MenuResponse>>>;
