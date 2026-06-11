using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(Guid Id, UpdateRoleRequest Request)
    : IRequest<BaseResponse<RoleDetailResponse>>;
