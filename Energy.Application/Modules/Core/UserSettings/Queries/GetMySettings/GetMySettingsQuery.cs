using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UserSettings.Queries.GetMySettings;

/// <summary>GetMySettings</summary>
public sealed record GetMySettingsQuery()
    : IRequest<BaseResponse<UserSettingsResponse>>;
