using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.Role.Commands.UpdateRole;

/// <summary>UpdateRole</summary>
public sealed record UpdateRoleCommand(Guid Id, UpdateRoleRequest Request)
    : IRequest<BaseResponse<RoleDetailResponse>>;
