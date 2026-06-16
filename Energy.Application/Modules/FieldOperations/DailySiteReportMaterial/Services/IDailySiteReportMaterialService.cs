using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Services;

/// <summary>DailySiteReportMaterial CRUD use-case sözleşmesi.</summary>
public interface IDailySiteReportMaterialService
{
    /// <summary>Sayfalanmış DailySiteReportMaterial listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>> GetListAsync(GetDailySiteReportMaterialListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DailySiteReportMaterialDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportMaterialRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportMaterialRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
