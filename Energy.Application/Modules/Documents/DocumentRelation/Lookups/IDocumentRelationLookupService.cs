using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;

namespace Energy.Application.Modules.Documents.DocumentRelation.Lookups;

/// <summary>DocumentRelation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDocumentRelationLookupService
{
    /// <summary>DocumentRelation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
