using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

namespace Energy.Application.Modules.Documents.DocumentPermission.Services;

/// <summary>DocumentPermission CRUD use-case sözleşmesi.</summary>
public interface IDocumentPermissionService
{
    /// <summary>Sayfalanmış DocumentPermission listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>> GetListAsync(GetDocumentPermissionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DocumentPermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDocumentPermissionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentPermissionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
