using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(CreateRoleRequest Request)
    : IRequest<BaseResponse<RoleDetailResponse>>;
