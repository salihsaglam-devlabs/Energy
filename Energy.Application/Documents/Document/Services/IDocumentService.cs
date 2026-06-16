using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Requests;
using Energy.Shared.Models.V1.Documents.Document.Responses;

namespace Energy.Application.Documents.Document.Services;

/// <summary>Document CRUD use-case sözleşmesi.</summary>
public interface IDocumentService
{
    /// <summary>Sayfalanmış Document listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DocumentListResponse>>> GetListAsync(GetDocumentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DocumentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDocumentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
