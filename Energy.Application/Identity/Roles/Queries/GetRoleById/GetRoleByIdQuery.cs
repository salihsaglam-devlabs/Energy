using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid Id) : IRequest<BaseResponse<RoleDetailResponse>>;
