using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

namespace Energy.Application.FieldOperations.ProgressEntry.Lookups;

/// <summary>ProgressEntry lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProgressEntryLookupService
{
    /// <summary>ProgressEntry lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
