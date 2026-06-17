using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.Permission.Queries.GetPermissionList;

/// <summary>GetPermissionList</summary>
public sealed record GetPermissionListQuery()
    : IRequest<BaseResponse<IReadOnlyList<PermissionResponse>>>;
