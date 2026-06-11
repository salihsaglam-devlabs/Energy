using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Localization;

public sealed class LocalizationApiClient : ApiClientBase, ILocalizationApiClient
{
    public LocalizationApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>(
            ApiRoutes.Localization.Base, cancellationToken);

    public Task<BaseResponse<LocalizationEntryResponse>> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<LocalizationEntryResponse>>(
            ApiRoutes.Localization.ByKey(key), cancellationToken);

    public Task<BaseResponse<LocalizationEntryResponse>> UpsertAsync(
        UpsertLocalizationEntryRequest request,
        CancellationToken cancellationToken = default)
        => PostAsync<UpsertLocalizationEntryRequest, BaseResponse<LocalizationEntryResponse>>(
            ApiRoutes.Localization.Base, request, cancellationToken);

    public Task<BaseResponse<string>> DeleteAsync(
        string key,
        CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<string>>(
            ApiRoutes.Localization.ByKey(key), cancellationToken);

    public Task<BaseResponse<SeedResultResponse>> ImportFromResxAsync(
        CancellationToken cancellationToken = default)
        => PostAsync<BaseResponse<SeedResultResponse>>(
            ApiRoutes.Localization.ImportFromResx, cancellationToken);
}

