using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Services;

/// <summary>MaterialCategory CRUD use-case sözleşmesi.</summary>
public interface IMaterialCategoryService
{
    /// <summary>Sayfalanmış MaterialCategory listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>> GetListAsync(GetMaterialCategoryListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialCategoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
