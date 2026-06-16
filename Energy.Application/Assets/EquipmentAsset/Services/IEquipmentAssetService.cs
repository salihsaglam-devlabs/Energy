using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;

namespace Energy.Application.Assets.EquipmentAsset.Services;

/// <summary>EquipmentAsset CRUD use-case sözleşmesi.</summary>
public interface IEquipmentAssetService
{
    /// <summary>Sayfalanmış EquipmentAsset listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>> GetListAsync(GetEquipmentAssetListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EquipmentAssetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentAssetRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentAssetRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
