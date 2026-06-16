using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Queries.GetUserProfileImage;

/// <summary><see cref="GetUserProfileImageQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetUserProfileImageQueryHandler
    : IRequestHandler<GetUserProfileImageQuery, ProfileImageResponse?>
{
    private readonly IUserService _users;

    public GetUserProfileImageQueryHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<ProfileImageResponse?> Handle(GetUserProfileImageQuery request, CancellationToken ct)
    {
        return await _users.GetProfileImageAsync(request.Id, ct);
    }
}
