using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Services;

/// <summary>DailySiteReportEquipment CRUD use-case sözleşmesi.</summary>
public interface IDailySiteReportEquipmentService
{
    /// <summary>Sayfalanmış DailySiteReportEquipment listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>> GetListAsync(GetDailySiteReportEquipmentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DailySiteReportEquipmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportEquipmentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportEquipmentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
