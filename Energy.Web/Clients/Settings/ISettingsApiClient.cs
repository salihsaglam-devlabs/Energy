using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;

namespace Energy.Web.Clients.Settings;

public interface ISettingsApiClient
{
    Task<BaseResponse<UserSettingsResponse>> GetMineAsync(CancellationToken ct = default);
    Task<BaseResponse<UserSettingsResponse>> UpdateMineAsync(UpdateUserSettingsRequest request, CancellationToken ct = default);
}

