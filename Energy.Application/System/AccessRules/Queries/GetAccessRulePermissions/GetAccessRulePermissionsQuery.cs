using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetAccessRulePermissions;

public sealed class GetAccessRulePermissionsQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    public Guid AccessRuleId { get; init; }

    public GetAccessRulePermissionsQuery(Guid accessRuleId)
    {
        AccessRuleId = accessRuleId;
    }
}

