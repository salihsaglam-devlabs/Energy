using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Services;

/// <summary>WorkOrderChecklistItem CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderChecklistItemService
{
    /// <summary>Sayfalanmış WorkOrderChecklistItem listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>> GetListAsync(GetWorkOrderChecklistItemListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderChecklistItemDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderChecklistItemRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderChecklistItemRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
