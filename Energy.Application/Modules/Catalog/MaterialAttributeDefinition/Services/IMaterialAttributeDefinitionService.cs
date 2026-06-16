using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Services;

/// <summary>MaterialAttributeDefinition CRUD use-case sözleşmesi.</summary>
public interface IMaterialAttributeDefinitionService
{
    /// <summary>Sayfalanmış MaterialAttributeDefinition listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>> GetListAsync(GetMaterialAttributeDefinitionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialAttributeDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
