using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Services;

/// <summary>WorkOrderChecklist CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderChecklistService
{
    /// <summary>Sayfalanmış WorkOrderChecklist listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>> GetListAsync(GetWorkOrderChecklistListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderChecklistDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderChecklistRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderChecklistRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
