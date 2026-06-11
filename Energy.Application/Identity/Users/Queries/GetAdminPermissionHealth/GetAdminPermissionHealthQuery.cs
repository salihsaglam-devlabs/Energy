using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetAdminPermissionHealth;

public sealed record GetAdminPermissionHealthQuery : IRequest<BaseResponse<AdminPermissionHealthResponse>>;

