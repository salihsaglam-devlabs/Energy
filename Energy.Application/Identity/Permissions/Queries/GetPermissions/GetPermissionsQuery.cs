using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Queries.GetPermissions;

public sealed class GetPermissionsQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<PermissionResponse>>>;
