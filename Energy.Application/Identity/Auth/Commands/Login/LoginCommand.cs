using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Auth.Commands.Login;

public sealed record LoginCommand(LoginRequest Request)
    : IRequest<BaseResponse<AuthTokenResponse>>;

