using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;

namespace Energy.Web.Clients.Localization;

public interface ILocalizationApiClient
{
    Task<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<BaseResponse<LocalizationEntryResponse>> UpsertAsync(UpsertLocalizationEntryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(string key, CancellationToken ct = default);
}
