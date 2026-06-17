using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;

namespace Energy.Application.Documents.DocumentFolder.Lookups;

/// <summary>DocumentFolder lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDocumentFolderLookupService
{
    /// <summary>DocumentFolder lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentFolderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
