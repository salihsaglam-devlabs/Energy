using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Services;

/// <summary>WorkOrderMaterialPlan CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderMaterialPlanService
{
    /// <summary>Sayfalanmış WorkOrderMaterialPlan listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>> GetListAsync(GetWorkOrderMaterialPlanListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderMaterialPlanDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialPlanRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialPlanRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
