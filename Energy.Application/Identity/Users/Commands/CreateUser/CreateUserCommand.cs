using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.CreateUser;

public sealed record CreateUserCommand(CreateUserRequest Request)
    : IRequest<BaseResponse<UserDetailResponse>>;
