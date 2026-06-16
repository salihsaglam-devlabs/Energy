using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using Energy.Application.Identity.Services;
using Energy.Application.Settings.Services;
using MediatR;

namespace Energy.Application.Modules.Core.UserSettings.Queries.GetMySettings;

/// <summary><see cref="GetMySettingsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetMySettingsQueryHandler
    : IRequestHandler<GetMySettingsQuery, BaseResponse<UserSettingsResponse>>
{
    private readonly IUserSettingsService _settings;
    private readonly ICurrentUser _currentUser;

    public GetMySettingsQueryHandler(IUserSettingsService settings, ICurrentUser currentUser)
    {
        _settings = settings;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<UserSettingsResponse>> Handle(GetMySettingsQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _settings.GetAsync(currentUserId, ct);
        return BaseResponse<UserSettingsResponse>.Success(result);
    }
}
