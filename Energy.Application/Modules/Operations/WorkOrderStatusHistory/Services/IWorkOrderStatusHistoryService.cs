using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Services;

/// <summary>WorkOrderStatusHistory CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderStatusHistoryService
{
    /// <summary>Sayfalanmış WorkOrderStatusHistory listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>> GetListAsync(GetWorkOrderStatusHistoryListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderStatusHistoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderStatusHistoryRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderStatusHistoryRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
