using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;

namespace Energy.Application.Localization.Services;

public interface ILocalizationService
{
    Task<IReadOnlyList<LocalizationEntryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<LocalizationEntryResponse?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the supplied values to the database and (when enabled) mirrors
    /// them into the corresponding .resx files. Returns the consolidated state
    /// after the write.
    /// </summary>
    Task<LocalizationEntryResponse> UpsertAsync(
        UpsertLocalizationEntryRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// One-shot import: reads every (culture, key, value) tuple from the
    /// on-disk .resx files and inserts/updates them in the database. Returns
    /// the number of rows added and updated.
    /// </summary>
    Task<SeedResultResponse> ImportFromResxAsync(CancellationToken cancellationToken = default);
}

