using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.CreatePermission;

public sealed record CreatePermissionCommand(CreatePermissionRequest Request)
    : IRequest<BaseResponse<PermissionResponse>>;
