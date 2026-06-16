using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Commands.RemoveUserProfileImage;

/// <summary><see cref="RemoveUserProfileImageCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class RemoveUserProfileImageCommandHandler
    : IRequestHandler<RemoveUserProfileImageCommand, BaseResponse<bool>>
{
    private readonly IUserService _users;

    public RemoveUserProfileImageCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<bool>> Handle(RemoveUserProfileImageCommand request, CancellationToken ct)
    {
        var ok = await _users.RemoveProfileImageAsync(request.Id, ct);
        if (!ok) throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, request.Id);
        return BaseResponse<bool>.Success(true);
    }
}
