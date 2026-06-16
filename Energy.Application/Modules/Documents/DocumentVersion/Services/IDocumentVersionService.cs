using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

namespace Energy.Application.Modules.Documents.DocumentVersion.Services;

/// <summary>DocumentVersion CRUD use-case sözleşmesi.</summary>
public interface IDocumentVersionService
{
    /// <summary>Sayfalanmış DocumentVersion listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DocumentVersionListResponse>>> GetListAsync(GetDocumentVersionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DocumentVersionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDocumentVersionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentVersionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
