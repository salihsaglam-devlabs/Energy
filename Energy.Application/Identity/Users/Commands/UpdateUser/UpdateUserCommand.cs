using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, UpdateUserRequest Request)
    : IRequest<BaseResponse<UserDetailResponse>>;
