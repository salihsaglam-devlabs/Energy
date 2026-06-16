using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Responses;

namespace Energy.Application.Modules.Documents.Document.Lookups;

/// <summary>Document lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDocumentLookupService
{
    /// <summary>Document lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
