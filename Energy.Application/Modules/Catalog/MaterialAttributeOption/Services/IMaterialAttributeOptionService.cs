using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Services;

/// <summary>MaterialAttributeOption CRUD use-case sözleşmesi.</summary>
public interface IMaterialAttributeOptionService
{
    /// <summary>Sayfalanmış MaterialAttributeOption listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>> GetListAsync(GetMaterialAttributeOptionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialAttributeOptionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeOptionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeOptionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
