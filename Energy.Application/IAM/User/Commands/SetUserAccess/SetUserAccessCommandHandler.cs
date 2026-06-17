using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.User.Commands.SetUserAccess;

/// <summary><see cref="SetUserAccessCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SetUserAccessCommandHandler
    : IRequestHandler<SetUserAccessCommand, BaseResponse<UserAccessResponse>>
{
    private readonly IUserService _users;

    public SetUserAccessCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<UserAccessResponse>> Handle(SetUserAccessCommand request, CancellationToken ct)
    {
        var result = await _users.SetAccessAsync(request.Id, request.Request, ct);
        return BaseResponse<UserAccessResponse>.Success(result);
    }
}
