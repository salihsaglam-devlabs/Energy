using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.SetAccessRulePermissions;

public sealed record SetAccessRulePermissionsCommand(Guid AccessRuleId, IReadOnlyCollection<Guid> PermissionIds)
    : IRequest<BaseResponse<IReadOnlyList<PermissionResponse>>>;

