using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Services;

/// <summary>EquipmentAssignment CRUD use-case sözleşmesi.</summary>
public interface IEquipmentAssignmentService
{
    /// <summary>Sayfalanmış EquipmentAssignment listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>> GetListAsync(GetEquipmentAssignmentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EquipmentAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentAssignmentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentAssignmentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
