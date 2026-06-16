using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.Auth.Commands.Login;

/// <summary>Login</summary>
public sealed record LoginCommand(LoginRequest Request)
    : IRequest<BaseResponse<AuthTokenResponse>>;
