using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

namespace Energy.Application.Modules.Operations.WorkOrder.Services;

/// <summary>WorkOrder CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderService
{
    /// <summary>Sayfalanmış WorkOrder listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderListResponse>>> GetListAsync(GetWorkOrderListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
