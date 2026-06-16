using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.User.Commands.SetUserProfileImage;

/// <summary><see cref="SetUserProfileImageCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SetUserProfileImageCommandHandler
    : IRequestHandler<SetUserProfileImageCommand, BaseResponse<bool>>
{
    private readonly IUserService _users;

    public SetUserProfileImageCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<bool>> Handle(SetUserProfileImageCommand request, CancellationToken ct)
    {
        var ok = await _users.SetProfileImageAsync(request.Id, Convert.FromBase64String(request.Request.ContentBase64), request.Request.ContentType, ct);
        if (!ok) throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, request.Id);
        return BaseResponse<bool>.Success(true);
    }
}
