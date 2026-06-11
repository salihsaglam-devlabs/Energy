using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.UpdateUserPassword;

public sealed record UpdateUserPasswordCommand(Guid Id, string NewPassword)
    : IRequest<BaseResponse<Guid>>;
