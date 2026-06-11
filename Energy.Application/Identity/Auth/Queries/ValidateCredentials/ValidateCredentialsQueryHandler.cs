using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Auth.Queries.ValidateCredentials;

public sealed class ValidateCredentialsQueryHandler
    : IRequestHandler<ValidateCredentialsQuery, BaseResponse<CredentialValidationResponse>>
{
    private readonly IUserService _userService;

    public ValidateCredentialsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<CredentialValidationResponse>> Handle(
        ValidateCredentialsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.ValidateCredentialsAsync(request.Request, cancellationToken);
        return BaseResponse<CredentialValidationResponse>.Success(result);
    }
}
