using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.UpdatePermission;

public sealed record UpdatePermissionCommand(Guid Id, UpdatePermissionRequest Request)
    : IRequest<BaseResponse<PermissionResponse>>;
