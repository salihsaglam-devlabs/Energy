using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;

namespace Energy.Application.Documents.DocumentFolder.Services;

/// <summary>DocumentFolder CRUD use-case sözleşmesi.</summary>
public interface IDocumentFolderService
{
    /// <summary>Sayfalanmış DocumentFolder listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DocumentFolderListResponse>>> GetListAsync(GetDocumentFolderListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DocumentFolderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDocumentFolderRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentFolderRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
