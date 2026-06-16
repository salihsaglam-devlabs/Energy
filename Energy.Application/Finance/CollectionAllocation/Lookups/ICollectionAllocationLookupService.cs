using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

namespace Energy.Application.Finance.CollectionAllocation.Lookups;

/// <summary>CollectionAllocation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ICollectionAllocationLookupService
{
    /// <summary>CollectionAllocation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
