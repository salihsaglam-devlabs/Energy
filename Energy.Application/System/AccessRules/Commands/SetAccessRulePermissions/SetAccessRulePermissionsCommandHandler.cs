using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.SetAccessRulePermissions;

public sealed class SetAccessRulePermissionsCommandHandler
    : IRequestHandler<SetAccessRulePermissionsCommand, BaseResponse<IReadOnlyList<PermissionResponse>>>
{
    private readonly IAccessRuleService _accessRuleService;

    public SetAccessRulePermissionsCommandHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<IReadOnlyList<PermissionResponse>>> Handle(
        SetAccessRulePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _accessRuleService.SetAccessRulePermissionsAsync(
            request.AccessRuleId,
            request.PermissionIds,
            cancellationToken);

        return BaseResponse<IReadOnlyList<PermissionResponse>>.Success(result);
    }
}

