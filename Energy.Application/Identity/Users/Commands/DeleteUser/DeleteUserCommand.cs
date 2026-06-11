using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : IRequest<BaseResponse<Guid>>;
