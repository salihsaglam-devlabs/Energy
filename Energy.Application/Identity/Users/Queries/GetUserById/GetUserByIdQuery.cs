using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<BaseResponse<UserDetailResponse>>;
