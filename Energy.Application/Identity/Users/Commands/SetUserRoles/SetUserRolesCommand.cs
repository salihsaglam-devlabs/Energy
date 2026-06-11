using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.SetUserRoles;

public sealed record SetUserRolesCommand(Guid Id, IReadOnlyList<Guid> RoleIds)
    : IRequest<BaseResponse<UserDetailResponse>>;
