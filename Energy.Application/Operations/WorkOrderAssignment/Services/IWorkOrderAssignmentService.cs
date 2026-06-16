using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Application.Operations.WorkOrderAssignment.Services;

/// <summary>WorkOrderAssignment CRUD use-case sözleşmesi.</summary>
public interface IWorkOrderAssignmentService
{
    /// <summary>Sayfalanmış WorkOrderAssignment listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>> GetListAsync(GetWorkOrderAssignmentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WorkOrderAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderAssignmentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderAssignmentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
