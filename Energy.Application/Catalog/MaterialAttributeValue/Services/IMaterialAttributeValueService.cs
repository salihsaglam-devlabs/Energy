using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

namespace Energy.Application.Catalog.MaterialAttributeValue.Services;

/// <summary>MaterialAttributeValue CRUD use-case sözleşmesi.</summary>
public interface IMaterialAttributeValueService
{
    /// <summary>Sayfalanmış MaterialAttributeValue listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>> GetListAsync(GetMaterialAttributeValueListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialAttributeValueDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeValueRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeValueRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
