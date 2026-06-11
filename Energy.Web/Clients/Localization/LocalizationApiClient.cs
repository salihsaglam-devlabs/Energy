using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Localization;

public sealed class LocalizationApiClient : ApiClientBase, ILocalizationApiClient
{
    public LocalizationApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>> GetAllAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>(ApiRoutes.Localization.Base, ct);

    public Task<BaseResponse<LocalizationEntryResponse>> UpsertAsync(UpsertLocalizationEntryRequest request, CancellationToken ct = default)
        => PostAsync<UpsertLocalizationEntryRequest, BaseResponse<LocalizationEntryResponse>>(ApiRoutes.Localization.Base, request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(string key, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Localization.ByKey(key), ct);
}
