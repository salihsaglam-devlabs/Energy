using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Responses;

namespace Energy.Application.Finance.Collection.Lookups;

/// <summary>Collection lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ICollectionLookupService
{
    /// <summary>Collection lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<CollectionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
