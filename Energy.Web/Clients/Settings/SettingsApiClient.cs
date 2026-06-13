using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Settings;

public sealed class SettingsApiClient : ApiClientBase, ISettingsApiClient
{
    public SettingsApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<UserSettingsResponse>> GetMineAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<UserSettingsResponse>>(ApiRoutes.Settings.Mine, ct);

    public Task<BaseResponse<UserSettingsResponse>> UpdateMineAsync(UpdateUserSettingsRequest request, CancellationToken ct = default)
        => PutAsync<UpdateUserSettingsRequest, BaseResponse<UserSettingsResponse>>(ApiRoutes.Settings.Mine, request, ct);
}

