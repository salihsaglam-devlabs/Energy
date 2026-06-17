using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

namespace Energy.Application.Documents.DocumentVersion.Lookups;

/// <summary>DocumentVersion lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDocumentVersionLookupService
{
    /// <summary>DocumentVersion lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
