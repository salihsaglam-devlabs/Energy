using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.Permission.Queries.GetPermissionByCode;

/// <summary>GetPermissionByCode</summary>
public sealed record GetPermissionByCodeQuery(string Code)
    : IRequest<BaseResponse<PermissionResponse>>;
