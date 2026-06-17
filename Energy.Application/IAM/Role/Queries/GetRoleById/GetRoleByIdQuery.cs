using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.Role.Queries.GetRoleById;

/// <summary>GetRoleById</summary>
public sealed record GetRoleByIdQuery(Guid Id)
    : IRequest<BaseResponse<RoleDetailResponse>>;
