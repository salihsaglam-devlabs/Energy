using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Queries.GetPermissionById;

public sealed record GetPermissionByIdQuery(Guid Id) : IRequest<BaseResponse<PermissionResponse>>;
