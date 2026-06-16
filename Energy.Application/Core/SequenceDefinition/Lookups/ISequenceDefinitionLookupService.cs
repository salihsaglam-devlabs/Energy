using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

namespace Energy.Application.Core.SequenceDefinition.Lookups;

/// <summary>SequenceDefinition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ISequenceDefinitionLookupService
{
    /// <summary>SequenceDefinition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
