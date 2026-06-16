using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.Role.Commands.SetRolePermissions;

/// <summary>SetRolePermissions</summary>
public sealed record SetRolePermissionsCommand(Guid Id, SetRolePermissionsRequest Request)
    : IRequest<BaseResponse<RoleDetailResponse>>;
