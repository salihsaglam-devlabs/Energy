using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

namespace Energy.Application.Organization.LeaveRequest.Services;

/// <summary>LeaveRequest CRUD use-case sözleşmesi.</summary>
public interface ILeaveRequestService
{
    /// <summary>Sayfalanmış LeaveRequest listesi.</summary>
    Task<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>> GetListAsync(GetLeaveRequestListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<LeaveRequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateLeaveRequestRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLeaveRequestRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
