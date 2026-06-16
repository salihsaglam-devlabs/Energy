using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.Role.Commands.DeleteRole;

/// <summary>DeleteRole</summary>
public sealed record DeleteRoleCommand(Guid Id)
    : IRequest<BaseResponse<bool>>;
