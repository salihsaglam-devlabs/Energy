using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;

namespace Energy.Web.Clients.Localization;

public interface ILocalizationApiClient
{
    Task<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<BaseResponse<LocalizationEntryResponse>> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<LocalizationEntryResponse>> UpsertAsync(
        UpsertLocalizationEntryRequest request,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<string>> DeleteAsync(
        string key,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<SeedResultResponse>> ImportFromResxAsync(
        CancellationToken cancellationToken = default);
}

