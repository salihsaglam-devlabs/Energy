using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Services;

/// <summary>EquipmentMaintenance CRUD use-case sözleşmesi.</summary>
public interface IEquipmentMaintenanceService
{
    /// <summary>Sayfalanmış EquipmentMaintenance listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>> GetListAsync(GetEquipmentMaintenanceListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EquipmentMaintenanceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentMaintenanceRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentMaintenanceRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
