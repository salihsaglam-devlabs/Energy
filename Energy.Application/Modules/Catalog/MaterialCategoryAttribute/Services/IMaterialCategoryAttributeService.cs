using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;

/// <summary>MaterialCategoryAttribute CRUD use-case sözleşmesi.</summary>
public interface IMaterialCategoryAttributeService
{
    /// <summary>Sayfalanmış MaterialCategoryAttribute listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>> GetListAsync(GetMaterialCategoryAttributeListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialCategoryAttributeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryAttributeRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryAttributeRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
