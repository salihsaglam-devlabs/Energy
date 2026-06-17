using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.User.Queries.GetMyProfile;

/// <summary><see cref="GetMyProfileQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetMyProfileQueryHandler
    : IRequestHandler<GetMyProfileQuery, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _users;
    private readonly ICurrentUser _currentUser;

    public GetMyProfileQueryHandler(IUserService users, ICurrentUser currentUser)
    {
        _users = users;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(GetMyProfileQuery request, CancellationToken ct)
    {
        // Kimliği yalnızca bağlamdan (token) çözeriz; istemci kimlik gönderemez.
        // Böylece bu uç noktayla bir kullanıcı asla başkasının kaydını okuyamaz.
        var currentUserId = _currentUser.UserId
            ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, Guid.Empty);

        var result = await _users.GetByIdAsync(currentUserId, ct)
            ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, currentUserId);

        return BaseResponse<UserDetailResponse>.Success(result);
    }
}

