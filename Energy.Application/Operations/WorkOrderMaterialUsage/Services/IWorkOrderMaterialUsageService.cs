using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Services;

/// <summary>WorkOrderMaterialUsage CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderMaterialUsageService
{
    /// <summary>Sayfalanmış WorkOrderMaterialUsage listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>> GetListAsync(GetWorkOrderMaterialUsageListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderMaterialUsageDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialUsageRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialUsageRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
