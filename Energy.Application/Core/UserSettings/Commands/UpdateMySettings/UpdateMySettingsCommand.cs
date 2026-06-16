using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using MediatR;

namespace Energy.Application.Core.UserSettings.Commands.UpdateMySettings;

/// <summary>UpdateMySettings</summary>
public sealed record UpdateMySettingsCommand(UpdateUserSettingsRequest Request)
    : IRequest<BaseResponse<UserSettingsResponse>>;
