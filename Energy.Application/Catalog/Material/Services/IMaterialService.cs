using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Requests;
using Energy.Shared.Models.V1.Catalog.Material.Responses;

namespace Energy.Application.Catalog.Material.Services;

/// <summary>Material CRUD use-case sözleşmesi.</summary>
public interface IMaterialService
{
    /// <summary>Sayfalanmış Material listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialListResponse>>> GetListAsync(GetMaterialListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
