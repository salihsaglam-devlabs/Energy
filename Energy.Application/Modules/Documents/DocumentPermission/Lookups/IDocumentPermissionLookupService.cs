using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

namespace Energy.Application.Modules.Documents.DocumentPermission.Lookups;

/// <summary>DocumentPermission lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDocumentPermissionLookupService
{
    /// <summary>DocumentPermission lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
